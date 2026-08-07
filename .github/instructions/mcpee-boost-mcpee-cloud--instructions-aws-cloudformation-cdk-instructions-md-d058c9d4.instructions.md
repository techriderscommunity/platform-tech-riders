---
applyTo: '**/*.{json,yaml,yml,ts,js,py}'
description: 'Best practices for AWS CloudFormation templates and AWS CDK constructs. Use when creating, reviewing, or suggesting CloudFormation or CDK infrastructure code.'
---

# AWS CloudFormation and CDK Best Practices

Source: [CloudFormation Best Practices](https://docs.aws.amazon.com/AWSCloudFormation/latest/UserGuide/best-practices.html) | [CDK Best Practices](https://docs.aws.amazon.com/cdk/v2/guide/best-practices.html)

---

## CloudFormation

### Template Structure

- Use `Description` to document every template's purpose.
- Declare all configurable values as `Parameters` with `AllowedValues`, `MinLength`/`MaxLength`, `Default`, and `Description`.
- Use `Mappings` for environment- or region-specific static values; never hardcode AMI IDs or account IDs.
- Use `Conditions` to create environment-aware resources (e.g., enable Multi-AZ only in prod).
- Use `Outputs` with `Export.Name` for cross-stack references. Keep exported names stable.
- Split large templates into nested stacks using `AWS::CloudFormation::Stack`.

### Security

- Never store secrets in template parameters or resource properties. Use `AWS::SecretsManager::Secret` or `AWS::SSM::Parameter` (SecureString).
- Apply least-privilege IAM policies. Avoid `*` actions and resources in production.
- Enable `EnableTerminationProtection` on production stacks.
- Use `DeletionPolicy: Retain` on stateful resources (RDS, S3, DynamoDB) to prevent accidental data loss.
- Tag all resources using `AWS::CloudFormation::Stack` tag inheritance.

### Reliability

- Use `DependsOn` only when implicit dependency ordering is insufficient.
- Use `UpdateReplacePolicy: Retain` alongside `DeletionPolicy: Retain` for stateful resources.
- Use `WaitCondition` or `cfn-signal` for EC2 bootstrapping to ensure rollback on failure.
- Prefer `Change Sets` over direct stack updates in production pipelines.

### Operational

- Use Stack Sets for multi-account and multi-region deployments.
- Use `AWS::CloudFormation::StackSet` with service-managed permissions (Organizations) for org-wide rollouts.
- Enable drift detection on critical stacks.
- Use `aws cloudformation deploy` with `--no-fail-on-empty-changeset` in CI/CD.

---

## AWS CDK (v2)

### Stack Design

- One CDK app per repository unless the workloads are truly independent.
- Keep stacks focused: separate network, compute, and data stacks.
- Pass cross-stack values via `stack.exportValue()` / `Fn.importValue()` or SSM Parameter Store — avoid direct object references between stacks in different environments.
- Use `cdk.Stage` to model environments (dev, staging, prod) within CDK Pipelines.

### Constructs

- Use L2 constructs (e.g., `aws_lambda.Function`, `aws_s3.Bucket`) over L1 (`CfnFunction`, `CfnBucket`) for guardrails and sensible defaults.
- Use L3 constructs (Patterns) for common patterns: `ApplicationLoadBalancedFargateService`, `LambdaRestApi`.
- Keep construct IDs stable — changing them replaces the underlying resource.
- Prefer `Props` interfaces over constructor overloading. Use `Partial<Props>` for optional fields.

### Security

- Call `bucket.grantRead(fn)` / `table.grantReadWriteData(fn)` instead of writing raw IAM policies.
- Never disable `blockPublicAccess` on S3 buckets unless the use case is intentionally public.
- Use `RemovalPolicy.RETAIN` for production stateful resources.
- Enable encryption by default: `encryption: s3.BucketEncryption.S3_MANAGED` or KMS.

### Testing

- Use `assertions.Template.fromStack(stack)` to assert resource existence and properties.
- Test each environment stage independently.
- Run `cdk synth` in CI to catch synthesis errors before deployment.
- Use `cdk diff` in PR pipelines to surface infrastructure changes.

### CDK Pipelines

- Use `CodePipeline` construct from `aws-cdk-lib/pipelines` for self-mutating pipelines.
- Gate production stages with `ManualApprovalStep`.
- Use `ShellStep` for pre/post-deployment validation commands.
- Store sensitive config in SSM or Secrets Manager; reference via `ssm.StringParameter.valueForStringParameter`.
