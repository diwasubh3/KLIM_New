CREATE PROC [CLO].[spDeleteCustomView] @viewId INT

AS

DELETE vf FROM CLO.CustomViewField vf
INNER JOIN CLO.CustomView v ON vf.ViewId = v.ViewId
WHERE v.ViewId = @viewId

DELETE FROM CLO.CustomView
WHERE ViewId = @viewId