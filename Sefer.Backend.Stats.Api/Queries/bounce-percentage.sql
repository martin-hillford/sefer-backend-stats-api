SELECT s.c / GREATEST(m.c + s.c, 1) * 100 AS BouncePercentage
FROM (
         SELECT CAST(COUNT(m.browser_token) AS DOUBLE PRECISION) AS c
         FROM (
              SELECT browser_token
              FROM client_page_request_log_entries
              WHERE browser_token IS NOT NULL $1
              GROUP BY browser_token
              HAVING COUNT("browser_token") > 1
         ) AS m
     ) AS m,
     (
         SELECT CAST(COUNT(s.browser_token) AS DOUBLE PRECISION) AS c
         FROM (
              SELECT browser_token
              FROM client_page_request_log_entries
              WHERE browser_token IS NOT NULL $1
              GROUP BY browser_token
              HAVING COUNT("browser_token") = 1
          ) AS s
     ) AS s