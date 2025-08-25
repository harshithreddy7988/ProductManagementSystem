using Amazon.CDK.AWS.CodeBuild;
using Amazon.CDK.AWS.CodePipeline.Actions;
using Amazon.CDK.AWS.CodePipeline;
using Amazon.CDK;

namespace ProductManagementSystemCdk.src
{
    public class PipelineStack : Stack
    {
        public PipelineStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var sourceOutput = new Artifact_();

            var pipeline = new Pipeline(this, "Pipeline", new PipelineProps
            {
                PipelineName = "ProductManagementPipeline"
            });

            pipeline.AddStage(new StageProps
            {
                StageName = "Source",
                Actions = new[] {
                new GitHubSourceAction(new GitHubSourceActionProps {
                    ActionName = "GitHub_Source",
                    Output = sourceOutput,
                    Repo = "ProductManagementSystem",
                    Owner = "YourGitHubUsername",
                    Branch = "main",
                    OAuthToken = SecretValue.SecretsManager("github-token")
                })
            }
            });

            pipeline.AddStage(new StageProps
            {
                StageName = "Build",
                Actions = new[] {
                new CodeBuildAction(new CodeBuildActionProps {
                    ActionName = "Build",
                    Project = new PipelineProject(this, "BuildProject"),
                    Input = sourceOutput,
                    Outputs = new[] { new Artifact_() }
                })
            }
            });

            pipeline.AddStage(new StageProps
            {
                StageName = "DeployToStaging",
                Actions = new[] {
                new ManualApprovalAction(new ManualApprovalActionProps {
                    ActionName = "ManualApproval"
                })
                // Add deploy action here using CDK pipeline or CloudFormation
            }
            });
        }
    }

}
