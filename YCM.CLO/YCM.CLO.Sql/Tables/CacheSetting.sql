CREATE TABLE CLO.CacheSetting
(
    CacheSettingId INT NOT NULL IDENTITY(1, 1)
	, CacheSettingKey VARCHAR(255) NOT NULL
	, CacheExpirationInSeconds INT NOT NULL DEFAULT(0)
	, CONSTRAINT PK_CacheSetting PRIMARY KEY(CacheSettingId)
	, CONSTRAINT UX_CacheSetting_CacheSettingKey UNIQUE(CacheSettingKey)
)
