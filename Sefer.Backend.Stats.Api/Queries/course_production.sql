SELECT
    c.id,
    c.name,
    NULLIF(done.count,0) AS done,
    NULLIF(cancelled.count, 0) AS cancelled,
    NULLIF(inactive.count, 0) AS in_active,
    NULLIF(active.count, 0) AS active
FROM Courses AS c
LEFT OUTER JOIN
(
     SELECT r.course_id as course,  COUNT(e.id) AS count FROM enrollments AS e
     JOIN course_revisions AS r ON r.id = e.course_revision_id
     WHERE is_course_completed = TRUE
     GROUP BY r.course_id
) AS done
ON c.Id = done.course

LEFT OUTER JOIN
(
     SELECT r.course_id as course,  COUNT(e.Id) AS count FROM enrollments AS e
     JOIN course_revisions AS r ON r.Id = e.course_revision_id
     WHERE is_course_completed = FALSE AND closure_date IS NOT NUll
     GROUP BY r.course_id
) AS cancelled
ON c.Id = cancelled.course

LEFT OUTER JOIN
(
    SELECT r.course_id AS course,  COUNT(e.Id) AS count FROM enrollments AS e
    JOIN course_revisions AS r ON r.Id = e.course_revision_id
    WHERE
        is_course_completed = FALSE AND
        closure_date IS NUll AND
        e.course_revision_id IN
        (
            SELECT DISTINCT e.course_revision_id FROM lesson_submissions
            JOIN enrollments AS e on lesson_submissions.enrollment_id = e.id
            WHERE is_final = TRUE AND submission_date > (NOW() - (INTERVAL '$1 day'))
        )
    GROUP BY r.course_id
) AS inactive
ON c.Id = inactive.course
LEFT OUTER JOIN
(
    SELECT r.course_id as course,  COUNT(e.Id) AS count FROM enrollments AS e
    JOIN course_revisions AS r ON r.Id = e.course_revision_id
    WHERE
        is_course_completed = FALSE AND
        closure_date IS NUll AND
        e.course_revision_id IN
        (
            SELECT DISTINCT e.course_revision_id FROM lesson_submissions
            JOIN enrollments e ON lesson_submissions.enrollment_id = e.Id
            WHERE is_final = TRUE AND submission_date < (NOW() - (INTERVAL '$1 day'))
        )
    GROUP BY r.course_id
 ) AS active
 ON c.Id = active.course