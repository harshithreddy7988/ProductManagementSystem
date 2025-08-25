using Amazon.CDK;
using ProductManagementSystemCdk.src;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagementSystemCdk
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();

            var envUSEast = new Environment { Account = "YOUR_ACCOUNT_ID", Region = "us-east-1" };
            var envUSWest = new Environment { Account = "YOUR_ACCOUNT_ID", Region = "us-west-2" };

            var networkStack = new NetworkStack(app, "NetworkStack", new StackProps { Env = envUSEast });

            new ComputeStack(app, "ComputeStackStaging", networkStack.Vpc, new StackProps
            {
                Env = envUSEast
            });

            new ComputeStack(app, "ComputeStackProd", networkStack.Vpc, new StackProps
            {
                Env = envUSWest
            });

            new PipelineStack(app, "PipelineStack");

            app.Synth();

        }
    }
}
