using Amazon.CDK.AWS.EC2;
using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementSystemCdk.src
{
    public class NetworkStack : Stack
    {
        public Vpc Vpc { get; }

        public NetworkStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            Vpc = new Vpc(this, "Vpc", new VpcProps
            {
                MaxAzs = 2,
                NatGateways = 1
            });
        }
    }
}
