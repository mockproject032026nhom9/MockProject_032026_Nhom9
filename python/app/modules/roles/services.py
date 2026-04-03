from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession

from app.core.logger import logger
from app.modules.roles.models import Role

class RoleService:
    """Business logic for role management and system seeding."""
    
    async def seed_roles(self, db: AsyncSession):
        """Seed the roles table with mandatory system roles if they don't exist."""
        target_roles = [
            {"id": 1, "name": "Admin"},
            {"id": 2, "name": "Dispatcher"},
            {"id": 3, "name": "Customer"}
        ]
        
        for role_data in target_roles:
            result = await db.execute(select(Role).filter(Role.id == role_data["id"]))
            if not result.scalars().first():
                db.add(Role(**role_data))
                logger.info(f"Seeded role: {role_data['name']}")
        
        await db.commit()

role_service = RoleService()
