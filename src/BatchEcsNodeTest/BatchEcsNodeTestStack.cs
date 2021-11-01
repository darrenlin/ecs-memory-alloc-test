using Amazon.CDK;
using Amazon.CDK.AWS.ECS;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.Batch;

namespace BatchEcsNodeTest
{
    public class BatchEcsNodeTestStack : Stack
    {
        internal BatchEcsNodeTestStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            Vpc vpc = new Vpc(this, "VPC");

            // The code that defines your stack goes here
            var EcsCluster = new Cluster(this, "Cluster", new ClusterProps { Vpc = vpc });
            EcsCluster.AddCapacity("NodeGroup-001", new AddCapacityOptions
            {
                InstanceType = new InstanceType("m5.large"),
                DesiredCapacity = 2,
            });
            EcsCluster.AddCapacity("NodeGroup-002", new AddCapacityOptions
            {
                InstanceType = new InstanceType("r5.large"),
                DesiredCapacity = 2,
            });
            EcsCluster.AddCapacity("NodeGroup-003", new AddCapacityOptions
            {
                InstanceType = new InstanceType("c5.large"),
                DesiredCapacity = 2,
            });
            EcsCluster.AddCapacity("NodeGroup-004", new AddCapacityOptions
            {
                InstanceType = new InstanceType("r5.xlarge"),
                DesiredCapacity = 2,
            });
            EcsCluster.AddCapacity("NodeGroup-005", new AddCapacityOptions
            {
                InstanceType = new InstanceType("m5.xlarge"),
                DesiredCapacity = 2,
            });
            EcsCluster.AddCapacity("NodeGroup-006", new AddCapacityOptions
            {
                InstanceType = new InstanceType("c5.xlarge"),
                DesiredCapacity = 2,
            });

            EcsCluster.AddCapacity("NodeGroup-007", new AddCapacityOptions
            {
                InstanceType = new InstanceType("c5.2xlarge"),
                DesiredCapacity = 2
            });

            EcsCluster.AddCapacity("NodeGroup-008", new AddCapacityOptions
            {
                InstanceType = new InstanceType("c5.4xlarge"),
                DesiredCapacity = 2
            });
            var AwsManagedCe01 = new ComputeEnvironment(this, "CE-001", new ComputeEnvironmentProps
            {
                Managed = true,
                ComputeResources = new ComputeResources
                {
                    Vpc = vpc,
                    Type = ComputeResourceType.SPOT,
                    InstanceTypes = new InstanceType[] {
                        new InstanceType("m5.large")
                        },
                    MinvCpus = 0

                }
            });

            var AwsManagedCe02 = new ComputeEnvironment(this, "CE-002", new ComputeEnvironmentProps
            {
                Managed = true,
                ComputeResources = new ComputeResources
                {
                    Vpc = vpc,
                    Type = ComputeResourceType.SPOT,
                    InstanceTypes = new InstanceType[] {
                        new InstanceType("c5.large")
                        },
                    MinvCpus = 0

                }
            });

            var AwsManagedCe03 = new ComputeEnvironment(this, "CE-003", new ComputeEnvironmentProps
            {
                Managed = true,
                ComputeResources = new ComputeResources
                {
                    Vpc = vpc,
                    Type = ComputeResourceType.SPOT,
                    InstanceTypes = new InstanceType[] {
                        new InstanceType("r5.large")
                        },
                    MinvCpus = 0

                }
            });
            var AwsManagedCe04 = new ComputeEnvironment(this, "CE-004", new ComputeEnvironmentProps
            {
                Managed = true,
                ComputeResources = new ComputeResources
                {
                    Vpc = vpc,
                    Type = ComputeResourceType.SPOT,
                    InstanceTypes = new InstanceType[] {
                        new InstanceType("m5.xlarge")
                        },
                    MinvCpus = 0
                }
            });

        }
    }
}
