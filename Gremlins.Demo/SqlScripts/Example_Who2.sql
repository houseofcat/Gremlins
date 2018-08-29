DECLARE @temp TABLE(
    SPID INT,
    [Status] VARCHAR(MAX),
    [LOGIN] VARCHAR(MAX),
    HostName VARCHAR(MAX),
    BlkBy VARCHAR(MAX),
    DBName VARCHAR(MAX),
    Command VARCHAR(MAX),
    CPUTime INT,
    DiskIO INT,
    LastBatch VARCHAR(MAX),
    ProgramName VARCHAR(MAX),
    SPID_1 INT,
    REQUESTID INT
)

INSERT INTO @temp EXEC sp_who2

SELECT COUNT(ProgramName), ProgramName, [LOGIN]
FROM    @temp
GROUP BY ProgramName, [LOGIN]

SELECT  *
FROM    @temp
ORDER BY CPUTime DESC