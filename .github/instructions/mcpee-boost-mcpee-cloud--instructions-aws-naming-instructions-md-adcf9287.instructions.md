---
applyTo: '**/*.{ts,js,mjs,cjs,py,json,yml,yaml}'
description: 'AWS resource naming conventions based on AWS tagging best practices and Well-Architected Framework. Use when creating, reviewing, or suggesting names for AWS resources.'
---

# AWS Resource Naming Conventions

Source: [AWS Tagging Best Practices](https://docs.aws.amazon.com/general/latest/gr/aws_tagging.html) | [Well-Architected OPS01](https://docs.aws.amazon.com/wellarchitected/latest/framework/ops_org_cloud_model.html)

Always follow these rules when creating, suggesting, or reviewing AWS resource names.

---

## General Pattern

```
<workload>-<component>-<environment>-<region>-<instance>
```

**Component rules:**
- **Workload / app / project** — short descriptive name (e.g., `payments`, `navigator`)
- **Component** — resource role (e.g., `api`, `db`, `queue`, `bucket`, `lambda`)
- **Environment** — `prod`, `dev`, `qa`, `staging`, `test`
- **Region** — use AWS region short codes: `use1` (us-east-1), `usw2` (us-west-2), `euw1` (eu-west-1), `euc1` (eu-central-1), `apse1` (ap-southeast-1), etc.
- **Instance** — zero-padded number when multiple: `01`, `02`

> Some resource types impose character restrictions (e.g., S3 bucket names must be globally unique, lowercase, no underscores). See per-service notes below.

**General character rules:**
- Prefer lowercase letters and hyphens (`-`). No spaces.
- S3 buckets: lowercase, hyphens only, 3–63 chars, globally unique.
- Lambda functions: letters, numbers, hyphens, underscores; max 64 chars.
- IAM roles/policies: letters, numbers, `+`, `=`, `,`, `.`, `@`, `-`, `_`; max 64 chars (roles), 128 chars (policies).
- CloudFormation stacks: letters, numbers, hyphens; max 128 chars.
- DynamoDB tables: letters, numbers, hyphens, underscores, dots; 3–255 chars.
- Do not embed account IDs, access keys, or secrets in names.
- Avoid AWS-reserved prefixes: `aws-`, `amazon-`, `AWS`.

---

## Required Tags

Apply these tags to all taggable AWS resources:

| Tag Key | Description | Example |
|---------|-------------|---------|
| `Name` | Human-readable resource name following the pattern above | `payments-api-prod-use1` |
| `Environment` | Deployment environment | `prod` \| `dev` \| `staging` |
| `Owner` | Team or individual responsible | `platform-team` |
| `Project` | Workload or application name | `payments` |
| `CostCenter` | For billing allocation | `cc-1234` |
| `ManagedBy` | IaC tool managing this resource | `terraform` \| `cloudformation` \| `cdk` |

---

## Per-Service Examples

| Service | Pattern | Example |
|---------|---------|---------|
| S3 Bucket | `<workload>-<env>-<region>-<purpose>` | `payments-prod-use1-uploads` |
| Lambda | `<workload>-<component>-<env>` | `payments-process-order-prod` |
| DynamoDB Table | `<workload>-<entity>-<env>` | `payments-orders-prod` |
| SQS Queue | `<workload>-<purpose>-<env>.fifo` | `payments-events-prod.fifo` |
| SNS Topic | `<workload>-<event>-<env>` | `payments-order-created-prod` |
| API Gateway | `<workload>-<api>-<env>` | `payments-rest-api-prod` |
| IAM Role | `<workload>-<component>-<purpose>-role` | `payments-lambda-execution-role` |
| VPC | `<workload>-vpc-<env>-<region>` | `payments-vpc-prod-use1` |
| EKS Cluster | `<workload>-eks-<env>-<region>` | `platform-eks-prod-use1` |
| RDS Instance | `<workload>-<engine>-<env>` | `payments-pg-prod` |

---

## IaC Naming Conventions

### CloudFormation / CDK

- Stack names: `<workload>-<layer>-<env>` → `payments-api-prod`
- Logical IDs in CFN templates: PascalCase, no hyphens → `PaymentsOrdersTable`
- CDK constructs: PascalCase class names → `PaymentsApiStack`
- Exported outputs: `<StackName>-<ResourceName>` → `payments-api-prod-ApiUrl`

### Terraform

- Resource names: snake_case → `payments_orders_table`
- Module names: snake_case → `payments_lambda`
- Variable names: snake_case → `environment`, `aws_region`
