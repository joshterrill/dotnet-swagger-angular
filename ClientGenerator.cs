using System;
using System.Threading;
using System.Threading.Tasks;
using NSwag;
using NSwag.CodeGeneration;
using NSwag.CodeGeneration.TypeScript;
using Microsoft.AspNetCore.Builder;

public static class ClientTypescriptGenerator {
    
    public static void UseGeneratedClient(this IApplicationBuilder app, string url) {
        Task.Run(() => {
            Thread.Sleep(2000);

            SwaggerDocument document = null;
            int attemptCount = 0;

            while (document == null && attemptCount < 5) {
                attemptCount++;
                System.Console.WriteLine("Attempting to load swagger specification for client generation.");
                try {
                    document = SwaggerDocument.FromUrlAsync(url).Result;
                } catch {
                    System.Console.WriteLine("Failed to load swagger specification. Retry in 2 seconds.");
                    Thread.Sleep(2000);
                }
            }

            var settings = new SwaggerToTypeScriptClientGeneratorSettings {
                Template = TypeScriptTemplate.Angular,
                ClassName = "{controller}Client",
                InjectionTokenType = InjectionTokenType.InjectionToken
            };

            var generator = new SwaggerToTypeScriptClientGenerator(document, settings);

            var code = generator.GenerateFile();

            System.Console.WriteLine(code);
            System.IO.File.WriteAllText(@"./ClientApp/src/app/app.service.ts", code);
        });
    }

}