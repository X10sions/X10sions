using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc.ApplicationParts {
  public static class ApplicationPartManagerExtensions {

    public static IList<TypeInfo> Controllers(this ApplicationPartManager applicationPartManager) {
      var feature = new ControllerFeature();
      applicationPartManager.PopulateFeature(feature);
      return feature.Controllers;

    }

    //public static IList<CodeAnalysis.MetadataReference> MetadataReferences(this ApplicationPartManager applicationPartManager) {
    //  var feature = new MetadataReferenceFeature();
    //  applicationPartManager.PopulateFeature(feature);
    //  return feature.MetadataReferences;
    //}

    public static IList<TypeInfo> TagHelpers(this ApplicationPartManager applicationPartManager) {
      var feature = new TagHelperFeature();
      applicationPartManager.PopulateFeature(feature);
      return feature.TagHelpers;

    }

    public static IList<TypeInfo> ViewComponents(this ApplicationPartManager applicationPartManager) {
      var feature = new ViewComponentFeature();
      applicationPartManager.PopulateFeature(feature);
      return feature.ViewComponents;
    }

    public static IList<CompiledViewDescriptor> ViewsFeatures(this ApplicationPartManager applicationPartManager) {
      var feature = new ViewsFeature();
      applicationPartManager.PopulateFeature(feature);
      return feature.ViewDescriptors;
    }


  }
}
