using System;
using System.Reflection;

namespace EMS.Web.Worker.MongoSaver.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}