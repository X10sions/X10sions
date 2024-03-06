using IBM.Data.DB2.iSeries;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;

namespace xEFCore.xAS400.Infrastructure.Internal {
  public class AS400OptionsExtension : RelationalOptionsExtension {

    public AS400OptionsExtension() {
    }

    protected AS400OptionsExtension([NotNull] AS400OptionsExtension copyFrom)
    : base(copyFrom) {
      // NB: When adding new options, make sure to update the copy ctor below.
      _rowNumberPaging = copyFrom._rowNumberPaging;
      _namingConvention = copyFrom._namingConvention;
    }

    protected override RelationalOptionsExtension Clone() => new AS400OptionsExtension(this);

    bool? _rowNumberPaging;
    public virtual bool? RowNumberPaging => _rowNumberPaging;

    iDB2NamingConvention? _namingConvention;
    public virtual iDB2NamingConvention? NamingConvention => _namingConvention;

    public virtual AS400OptionsExtension WithNamingConvention(iDB2NamingConvention namingConvention) {
      var clone = (AS400OptionsExtension)Clone();
      clone._namingConvention = namingConvention;
      return clone;
    }

    public virtual AS400OptionsExtension WithRowNumberPaging(bool rowNumberPaging) {
      var clone = (AS400OptionsExtension)Clone();
      clone._rowNumberPaging = rowNumberPaging;
      return clone;
    }

    long? _serviceProviderHash;
    public override long GetServiceProviderHashCode() {
      if (_serviceProviderHash == null) {
        _serviceProviderHash = (base.GetServiceProviderHashCode() * 397) ^ (_rowNumberPaging?.GetHashCode() ?? 0L);
      }
      return _serviceProviderHash.Value;
    }

    public override bool ApplyServices(IServiceCollection services) {
      Check.NotNull(services, nameof(services));
      services.AddEntityFrameworkAS400x();
      return true;
    }

    string _logFragment;
    public override string LogFragment {
      get {
        if (_logFragment == null) {
          var builder = new StringBuilder();
          builder.Append(base.LogFragment);
          if (_rowNumberPaging == true) {
            builder.Append(nameof(RowNumberPaging) + " ");
          }
          _logFragment = builder.ToString();
        }
        return _logFragment;
      }
    }

  }
}