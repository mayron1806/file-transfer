namespace Domain.Plan;

public static class Plans
{
    public static Plan Free => new(
        name: "Free", 
        description: "Free plan",
        price: 0, // R$ 0,00
        limits: new Limits(
            maxUploadSize: 5e+9, // 5 GB
            maxStorageSize: 5e+9, // 5 GB
            maxEmails: 10, 
            canUseExpiresOnDownload: true, 
            canUseQuickDownload: true,
            canUsePassword: true,
            maxExpireDays: 7,
            maxUploadConcurrency: 500,
            maxUploadPerDay: 500
        )
    );
    public static Plan Starter => new(
        name: "Starter",
        description: "Starter plan",
        price: 3000, // R$ 30,00
        limits: new Limits(
            maxUploadSize: 2e+11, // 200 GB
            maxStorageSize: 4e+11, // 400 GB
            maxEmails: 50,
            canUseExpiresOnDownload: true,
            canUseQuickDownload: true,
            canUsePassword: true,
            maxExpireDays: 30,
            maxUploadConcurrency: 200,
            maxUploadPerDay: 1000
        )
    );
    public static Plan Pro => new(
        name: "Pro",
        description: "Pro plan",
        price: 10000, // R$ 100,00
        limits: new Limits(
            maxUploadSize: 3e+11, // 300 GB
            maxStorageSize: 1e+12, // 1 TB
            maxEmails: 100,
            canUseExpiresOnDownload: true,
            canUseQuickDownload: true,
            canUsePassword: true,
            maxExpireDays: 365,
            maxUploadConcurrency: 300,
            maxUploadPerDay: 10000
        )
    );
    public static Plan ProMax => new(
        name: "Pro Max",
        description: "Pro Max plan",
        price: 17000, // R$ 170,00
        limits: new Limits(
            maxUploadSize: 3e+11, // 300 GB
            maxStorageSize: 2e+12, // 2 TB
            maxEmails: 100,
            canUseExpiresOnDownload: true,
            canUseQuickDownload: true,
            canUsePassword: true,
            maxExpireDays: 365,
            maxUploadConcurrency: 300,
            maxUploadPerDay: 30000
        )
    );
}
