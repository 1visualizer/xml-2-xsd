using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Xml;
using System.Xml.Schema;
namespace onevisualizer.Function
{
    public static class xml2xsd
    {
        [FunctionName("xml2xsd")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,  "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            XmlReader reader = XmlReader.Create(new StreamReader(req.Body));
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            XmlSchemaInference schema = new XmlSchemaInference();

            schemaSet = schema.InferSchema(reader);

            var sb = new System.Text.StringBuilder();
            StringWriter strWriter = new StringWriter();
            XmlWriter writer;

            foreach (XmlSchema s in schemaSet.Schemas())
            {
                writer = XmlWriter.Create(strWriter);
                s.Write(writer);
                writer.Close();
            }
            reader.Close();

            return new OkObjectResult(strWriter.ToString());
        }
    }
}
