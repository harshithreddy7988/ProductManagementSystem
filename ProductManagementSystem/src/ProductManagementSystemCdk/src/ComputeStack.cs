using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ECS;
using Amazon.CDK.AWS.ElasticLoadBalancingV2;
using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementSystemCdk.src
{
    public class ComputeStack : Stack
    {
        public ComputeStack(Construct scope, string id, Vpc vpc, IStackProps props = null) : base(scope, id, props)
        {
            var cluster = new Cluster(this, "EcsCluster", new ClusterProps
            {
                Vpc = vpc
            });

            var taskDefinition = new FargateTaskDefinition(this, "TaskDef");

            taskDefinition.AddContainer("AppContainer", new ContainerDefinitionOptions
            {
                Image = ContainerImage.FromRegistry("amazon/amazon-ecs-sample"),
                MemoryLimitMiB = 512,
                Logging = LogDriver.AwsLogs(new AwsLogDriverProps
                {
                    StreamPrefix = "ecs"
                })
            });

            var service = new FargateService(this, "FargateService", new FargateServiceProps
            {
                Cluster = cluster,
                TaskDefinition = taskDefinition,
                DesiredCount = 2
            });

            var lb = new ApplicationLoadBalancer(this, "LB", new ApplicationLoadBalancerProps
            {
                Vpc = vpc,
                InternetFacing = true
            });

            var listener = lb.AddListener("PublicListener", new BaseApplicationListenerProps
            {
                Port = 80,
                Open = true
            });

            listener.AddTargets("ECS", new AddApplicationTargetsProps
            {
                Port = 80,
                Targets = new[] { service }
            });

            new CfnOutput(this, "LoadBalancerDNS", new CfnOutputProps
            {
                Value = lb.LoadBalancerDnsName
            });
        }
    }

}
