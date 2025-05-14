SELECT Bin, CAST(POWER(2,Bin) AS DECIMAL) / 1000 AS ProcessingTime, Count(*) AS Count
FROM
(
    SELECT FLOOR(LOG(processing_time) / LOG(2)) AS Bin
    FROM api_request_log_entries
    WHERE processing_time > 7
) AS Data
GROUP BY Bin
UNION
SELECT 0 AS BIN, 0 AS ProcessingTime, COUNT(*)
FROM api_request_log_entries WHERE processing_time < 8