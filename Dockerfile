FROM microsoft/dotnet:1.1-runtime-deps

COPY BackgroundJobsScheduler/BackgroundJobsScheduler/publish /home/bin
WORKDIR /home/bin

EXPOSE 3001/tcp

CMD ["./BackgroundJobsScheduler"]
