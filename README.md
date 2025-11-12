# Recurring Integrations Scheduler RIS

This tool helps to quickly set up and orchestrate file based integration scenarios to and from Dynamics 365 Finance and Dynamics 365 Supply Chain Management. We see this tool to be a good implementation accelerator for use during the implementation phase of the project for data migration, ad hoc file integration needs and to be used as a proof of concept validator among others. 

## Recurring Integrations Scheduler Service

Recurring Integrations Scheduler Service is a Windows service that can trigger many integration jobs at predefined schedule. You can schedule jobs and set their recurrence in a very similar way to well-known D365FO batch framework.

There are six types of integration jobs you can use: [Import](https://github.com/microsoft/Recurring-Integrations-Scheduler/wiki#import-job), [Export](https://github.com/microsoft/Recurring-Integrations-Scheduler/wiki#export-job) and [Execution monitor](https://github.com/microsoft/Recurring-Integrations-Scheduler/wiki#execution-monitor-job) as well as [Upload](https://github.com/microsoft/Recurring-Integrations-Scheduler/wiki#upload-job), [Download](https://github.com/microsoft/Recurring-Integrations-Scheduler/wiki#download-job) and [Processing monitor](https://github.com/microsoft/Recurring-Integrations-Scheduler/wiki#processing-monitor-job). First three use [Data management package REST API](https://docs.microsoft.com/en-us/dynamics365/fin-ops-core/dev-itpro/data-entities/data-management-api). Three other use [Recurring Integrations REST API](https://docs.microsoft.com/en-us/dynamics365/fin-ops-core/dev-itpro/data-entities/recurring-integrations) and recurring data jobs.
More details in wiki.

Single Recurring Integrations Scheduler service can work with multiple D365FO instances even within different tenants. This enables both production and implementation scenarios where you need to work with multiple non-prod instances. Recurring Integrations Scheduler logs important events to Windows Event Log. It is possible to increase its logging level to log every step for debugging purposes and to trace possible problems.

## Recurring Integrations Scheduler App

Recurring Integrations Scheduler App is a win32 application that can be used as a configuration front-end for Recurring Integrations Scheduler service or as a completely independent, interactive application used to upload or download files to and from Dynamics 365 Finance or SCM without Recurring Integrations Scheduler Service.
It is possible thanks to internal, private scheduler embedded in Recurring Integrations Scheduler App that works exactly the same way as the Scheduler Service does with one difference - it will stop working once the App is closed.

## SFTP Integration

Upload and Import jobs can now pull source files from an SFTP drop before processing, while Download and Export jobs can push their output to an SFTP target once D365 processing succeeds. Each job exposes an **SFTP** tab in the Scheduler UI where you can provide host, port, username, and either a password or key file plus the remote folder and file mask. Secrets are encrypted when saved to the job definition. Inbound transfers delete the remote blob after a successful download; outbound transfers move the local artifact into an `Uploaded` subfolder after it lands on SFTP so you can audit what was shipped.

## Installation and configuration

Please check https://github.com/Microsoft/Recurring-Integrations-Scheduler/wiki

## Support

RIS is not officially supported by Microsoft. Please log an issue here on Github if you encounter a bug in RIS.

## Testing

A lightweight MSTest project lives under `Common.Tests`. You can run it from Visual Studio's Test Explorer or via the command line:

```powershell
dotnet test Common.Tests\Common.Tests.csproj
```

The tests exercise core helpers (file handling, zip extraction safeguards) and provide a starting point for broader coverage.
