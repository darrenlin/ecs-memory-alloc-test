# Welcome to your CDK C# project!

This is a blank project for C# development with CDK.

The `cdk.json` file tells the CDK Toolkit how to execute your app.

It uses the [.NET Core CLI](https://docs.microsoft.com/dotnet/articles/core/) to compile and execute your project.

## Useful commands

* `dotnet build src` compile this app
* `cdk deploy`       deploy this stack to your default AWS account/region
* `cdk diff`         compare deployed stack with current state
* `cdk synth`        emits the synthesized CloudFormation template

1. ECS Cluster 

https://docs.aws.amazon.com/AmazonECS/latest/developerguide/memory-management.html

For example, an m4.large instance has 8 GiB of installed memory. However, this does not always translate to exactly 8192 MiB of memory available for tasks when the container instance registers.

The Amazon ECS container agent uses the Docker ReadMemInfo() function to query the total memory available to the operating system. Both Linux and Windows provide command line utilities to determine the total memory.

The free command returns the total memory that is recognized by the operating system.

The Amazon ECS container agent provides a configuration variable called ECS_RESERVED_MEMORY, which you can use to remove a specified number of MiB of memory from the pool that is allocated to your tasks. This effectively reserves that memory for critical system processes.

2. EC2 Instance Memory

https://aws.amazon.com/ec2/faqs/?nc1=h_ls

Q: Why does the total memory reported by the operating system not exactly match the advertised memory on instance types?

Portions of the EC2 instance memory are reserved and used by the virtual BIOS for video RAM, DMI, and ACPI. In addition, for instances that are powered by the AWS Nitro Hypervisor, a small percentage of the instance memory is reserved by the Amazon EC2 Nitro Hypervisor to manage virtualization.

https://access.redhat.com/solutions/59723

> * mem_map[]: This is a kernel structure used to manage and reclaim memory. It is also the largest _consumer_ of memory, and it grows larger as the amount of installed RAM increases. It uses ~1.3-1.5% of memory on 64-bit and ~0.78-1% on 32-bit kernels. It is sometimes possible to decrease this amount by reconfiguring and recompiling your kernel to remove certain kernel features like kmemcheck, but it is not possible to change it if you are, for example, running a fixed distribution kernel.

> Also, Hypervisors will tend to eat lots of memory. Running a KVM guest, you'll pay that ~1.3-1.5% for mem_map[] twice: once in the host, and once in the guest.

3. USER_DATA in ECS Instnace, Launched by AWS Batch CE

```
Content-Type: multipart/mixed; boundary="==BOUNDARY==" 
MIME-Version: 1.0 

--==BOUNDARY== 
MIME-Version: 1.0 
Content-Type: text/x-shellscript; charset="us-ascii"

#!/bin/bash 
echo ECS_CLUSTER=CE001C1D74FE3-53239f7c4017500_Batch_db1f2a3c-bafa-3f50-9fde-2297cd92ddd5>>/etc/ecs/ecs.config 
echo ECS_DISABLE_IMAGE_CLEANUP=false>>/etc/ecs/ecs.config 
echo ECS_ENGINE_TASK_CLEANUP_WAIT_DURATION=2m>>/etc/ecs/ecs.config 
echo ECS_IMAGE_CLEANUP_INTERVAL=10m>>/etc/ecs/ecs.config 
echo ECS_IMAGE_MINIMUM_CLEANUP_AGE=10m>>/etc/ecs/ecs.config 
echo ECS_NUM_IMAGES_DELETE_PER_CYCLE=5>>/etc/ecs/ecs.config
echo ECS_RESERVED_MEMORY=32>>/etc/ecs/ecs.config
--==BOUNDARY==
MIME-Version: 1.0
Content-Type: text/x-shellscript; charset="us-ascii"

#!/bin/bash
echo ECS_CLUSTER=CE001C1D74FE3-53239f7c4017500_Batch_db1f2a3c-bafa-3f50-9fde-2297cd92ddd5>>/etc/ecs/ecs.config

--==BOUNDARY==--
```

4. Examples 

```

## Lanuched with CDK
-----------------------------------------
|      DescribeContainerInstances       |
+---------------+-----------------------+
| InstanceType  |  registeredResources  |
+---------------+-----------------------+
|  c5.large     |  3731                 |
|  m5.large     |  7763                 |
|  c5.xlarge    |  7623                 |
|  r5.large     |  15743                |
|  m5.xlarge    |  15575                |
|  r5.xlarge    |  31703                |
+---------------+-----------------------+

```
```
c5.large        3731        8.91%
c5.xlarge       7623        6.94%
c5.2xlarge      15574       4.94%
c5.4xlarge      31141       4.96%

m5.large        7763        5.23%
m5.xlarge       15575       4.94%
r5.large        15743       3.91%
r5.xlarge       31703       3.25%
```

```
m5.large        7763
m5.large        7763
c5.large        3703
c5.large        3731
r5.large        15743
r5.large        15743
m5.xlarge       15743
m5.xlarge       15575
c5.4xlarge      31141
c5.4xlarge      31141
r5.xlarge       31703
r5.xlarge       31703
c5.2xlarge      15574
c5.2xlarge      15574
c5.xlarge       7623
c5.xlarge       7623
```

echo "(2^14-15574)/2^14*100" | bc -l

## Launch with AWS Batch

c5.lrage        3700  --> Expected
m5.large        7648  --> diff 115; 115-32=83? (OS/EC2?)
r5.large        15712 --> 31
m5.xlarge       15712 --> diff 137?


sh-4.2$ cat /proc/cpuinfo | grep Intel
vendor_id	: GenuineIntel

## EC2 Instance 1
model name	: Intel(R) Xeon(R) Platinum 8259CL CPU @ 2.50GHz

[    0.000000] e820: BIOS-provided physical RAM map:
[    0.000000] BIOS-e820: [mem 0x0000000000000000-0x000000000009fbff] usable
[    0.000000] BIOS-e820: [mem 0x000000000009fc00-0x000000000009ffff] reserved
[    0.000000] BIOS-e820: [mem 0x00000000000f0000-0x00000000000fffff] reserved
[    0.000000] BIOS-e820: [mem 0x0000000000100000-0x00000000bffe9fff] usable
[    0.000000] BIOS-e820: [mem 0x00000000bffea000-0x00000000bfffffff] reserved
[    0.000000] BIOS-e820: [mem 0x00000000e0000000-0x00000000e03fffff] reserved
[    0.000000] BIOS-e820: [mem 0x00000000fffc0000-0x00000000ffffffff] reserved
[    0.000000] BIOS-e820: [mem 0x0000000100000000-0x00000002329fffff] usable

sh-4.2$ free -m
              total        used        free      shared  buff/cache   available
Mem:           7763         212        7153           0         398        7335
Swap:             0           0           0

model name	: Intel(R) Xeon(R) Platinum 8175M CPU @ 2.50GHz

sh-4.2$ dmesg | grep e820
[    0.000000] e820: BIOS-provided physical RAM map:
[    0.000000] BIOS-e820: [mem 0x0000000000000000-0x000000000009fbff] usable
[    0.000000] BIOS-e820: [mem 0x000000000009fc00-0x000000000009ffff] reserved
[    0.000000] BIOS-e820: [mem 0x00000000000f0000-0x00000000000fffff] reserved
[    0.000000] BIOS-e820: [mem 0x0000000000100000-0x00000000bffe9fff] usable
[    0.000000] BIOS-e820: [mem 0x00000000bffea000-0x00000000bfffffff] reserved
[    0.000000] BIOS-e820: [mem 0x00000000e0000000-0x00000000e03fffff] reserved
[    0.000000] BIOS-e820: [mem 0x00000000fffc0000-0x00000000ffffffff] reserved
[    0.000000] BIOS-e820: [mem 0x0000000100000000-0x000000022d3fffff] usable
[    0.000000] BIOS-e820: [mem 0x000000022d400000-0x000000023fffffff] reserved

sh-4.2$ free -m
              total        used        free      shared  buff/cache   available
Mem:           7679         206        7080           0         392        7255
Swap:             0           0           0
