from pydantic_settings import BaseSettings, SettingsConfigDict

class Settings(BaseSettings):
    """
    Global application configurations.
    Values can be overridden by environment variables or an .env file.
    """
    PROJECT_NAME: str = "FastAPI REST API"
    VERSION: str = "1.0.0"
    API_V1_STR: str = "/api/v1"
    
    SECRET_KEY: str = "DEV_SECRET_KEY"
    ALGORITHM: str = "HS256"
    ACCESS_TOKEN_EXPIRE_MINUTES: int = 30
    
    # DATABASE_URL: str = "sqlite+aiosqlite:///./test.db"

    DATABASE_URL: str = (
        "mssql+aioodbc://sa:123456@127.0.0.1/mockdb?"
        "driver=ODBC+Driver+18+for+SQL+Server&"
        "TrustServerCertificate=yes&Encrypt=no"
    )
    # DATABASE_URL: str = (
    #     "mssql+aioodbc://sa:123@localhost/mockdb?"
    #     "driver=ODBC+Driver+17+for+SQL+Server&"
    #     "TrustServerCertificate=yes"
    # )

    model_config = SettingsConfigDict(
        case_sensitive=True,
        env_file=".env",
        extra="ignore"
    )

settings = Settings()
