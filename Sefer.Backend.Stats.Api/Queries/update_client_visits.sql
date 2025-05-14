UPDATE client_page_request_log_entries
SET is_unique_visit = FALSE
WHERE Id IN
(
  SELECT s_id FROM
      (
          SELECT
              s.id AS s_id,
              t.id as t_id,
              EXTRACT(EPOCH FROM (s.log_time - t.log_time)) AS diff,
              s.browser_token AS s_token,
              t.browser_token AS t_token,
              s.path AS s_path,
              t.path AS t_path
          FROM client_page_request_log_entries AS s, client_page_request_log_entries AS t
          WHERE
              s.log_time >= (SELECT (MAX(log_time) - (INTERVAL '1 day')) FROM client_page_request_log_entries WHERE is_unique_visit = FALSE) AND
              t.log_time >= (SELECT (MAX(log_time) - (INTERVAL '1 day')) FROM client_page_request_log_entries WHERE is_unique_visit = FALSE)
      )
          AS x
  WHERE diff between 0 AND 1800 AND s_id != t_id AND s_token = t_token AND s_path = t_path AND s_token IS NOT NULL
)