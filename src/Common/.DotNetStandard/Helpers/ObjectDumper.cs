using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace Common.Helpers {  
  public class ObjectDumper {

    ObjectDumper(int depth) {
      this.depth = depth;
    }

    int depth;
    int level;
    int pos;
    TextWriter writer;

    public static void Write(object element) => Write(element, 0);

    public static void Write(object element, int depth) => Write(element, depth, Console.Out);

    public static void Write(object element, int depth, TextWriter log) => new ObjectDumper(depth) { writer = log }.WriteObject(null, element);

     void Write(string s) {
      if (s != null) {
        writer.Write(s);
        pos += s.Length;
      }
    }

     void WriteIndent() {
      for (int i = 0; i <= level - 1; i++)
        writer.Write("  ");
    }

     void WriteLine() {
      writer.WriteLine();
      pos = 0;
    }

     void WriteObject(string prefix, object element) {
      if (element == null || element is ValueType || element is string) {
        WriteIndent();
        Write(prefix);
        WriteValue(element);
        WriteLine();
      } else {
        IEnumerable enumerableElement = element as IEnumerable;
        if (enumerableElement != null) {
          foreach (object item in enumerableElement) {
            if (item is IEnumerable && !(item is string)) {
              WriteIndent();
              Write(prefix);
              Write("...");
              WriteLine();
              if (level < depth) {
                level += 1;
                WriteObject(prefix, item);
                level -= 1;
              }
            } else
              WriteObject(prefix, item);
          }
        } else {
          MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
          WriteIndent();
          Write(prefix);
          bool propWritten = false;
          foreach (var m in members) {
            var f = m as FieldInfo;
            var p = m as PropertyInfo;
            if (f != null || p != null) {
              if (propWritten)
                WriteTab();
              else
                propWritten = true;
              Write(m.Name);
              Write("=");
              var t = f != null ? f.FieldType : p.PropertyType;
              if (t.IsValueType || t == typeof(string))
                WriteValue(f != null ? f.GetValue(element) : p.GetValue(element, null));
              else if (typeof(IEnumerable).IsAssignableFrom(t))
                Write("...");
              else
                Write("{ }");
            }
          }
          if (propWritten)
            WriteLine();
          if (level < depth) {
            foreach (var m in members) {
              var f = m as FieldInfo;
              var p = m as PropertyInfo;
              if (f != null || p != null) {
                Type t = f != null ? f.FieldType : p.PropertyType;
                if (!(t.IsValueType || t == typeof(string))) {
                  object value = f != null ? f.GetValue(element) : p.GetValue(element, null);
                  if (value != null) {
                    level += 1;
                    WriteObject(m.Name + ": ", value);
                    level -= 1;
                  }
                }
              }
            }
          }
        }
      }
    }

     void WriteTab() {
      Write("  ");
      while (pos % 8 != 0)
        Write(" ");
    }

     void WriteValue(object o) {
      if (o == null)
        Write("null");
      else if (o is DateTime)
        Write(((DateTime)o).ToShortDateString());
      else if (o is ValueType || o is string)
        Write(o.ToString());
      else if (o is IEnumerable)
        Write("...");
      else
        Write("{ }");
    }
  }
}