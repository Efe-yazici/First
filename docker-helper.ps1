# Docker management script for Hasta Takip Sistemi (PowerShell version)
# Bu script Docker işlemlerini Windows'ta kolaylaştırmak için hazırlanmıştır

param(
    [Parameter(Position=0)]
    [string]$Command = "help",
    
    [Parameter(Position=1)]
    [string]$Service = "",
    
    [switch]$NoCache
)

$ProjectName = "hastaTakip"
$ComposeFile = "docker-compose.yml"

function Show-Help {
    Write-Host "Hasta Takip Sistemi Docker Yönetim Scripti" -ForegroundColor Green
    Write-Host ""
    Write-Host "Kullanım: .\docker-helper.ps1 [KOMUT] [SEÇENEKLER]" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Komutlar:" -ForegroundColor Cyan
    Write-Host "  start-db        Sadece SQL Server'ı başlat"
    Write-Host "  start-all       Tüm servisleri başlat (Windows containers gerekli)"
    Write-Host "  start-dev       Geliştirme ortamını başlat"
    Write-Host "  stop            Tüm servisleri durdur"
    Write-Host "  restart         Tüm servisleri yeniden başlat"
    Write-Host "  logs            Logları göster"
    Write-Host "  build           Uygulamayı yeniden build et"
    Write-Host "  clean           Tüm container ve volume'ları temizle"
    Write-Host "  status          Servis durumlarını göster"
    Write-Host "  db-init         Veritabanını manuel olarak başlat"
    Write-Host "  switch-windows  Windows containers moduna geç"
    Write-Host "  switch-linux    Linux containers moduna geç"
    Write-Host "  help            Bu yardım mesajını göster"
    Write-Host ""
    Write-Host "Örnekler:" -ForegroundColor Cyan
    Write-Host "  .\docker-helper.ps1 start-db                    # Sadece SQL Server'ı başlat"
    Write-Host "  .\docker-helper.ps1 start-all                   # Tüm sistemi başlat"
    Write-Host "  .\docker-helper.ps1 logs hastaTakip-sqlserver   # SQL Server loglarını göster"
    Write-Host "  .\docker-helper.ps1 build -NoCache              # Cache kullanmadan build et"
}

function Test-Docker {
    try {
        $null = Get-Command docker -ErrorAction Stop
        $null = Get-Command docker-compose -ErrorAction Stop
    }
    catch {
        Write-Error "Hata: Docker veya Docker Compose kurulu değil!"
        exit 1
    }
}

function Start-Database {
    Write-Host "SQL Server başlatılıyor..." -ForegroundColor Green
    docker-compose up -d sqlserver
    Write-Host "SQL Server başlatıldı. Bağlantı: localhost:1433" -ForegroundColor Green
    Write-Host "SA Şifresi: YourPassword123" -ForegroundColor Yellow
}

function Start-All {
    Write-Host "Tüm servisler başlatılıyor (Windows containers gerekli)..." -ForegroundColor Green
    docker-compose --profile production up -d
    Write-Host "Tüm servisler başlatıldı." -ForegroundColor Green
}

function Start-Development {
    Write-Host "Geliştirme ortamı başlatılıyor..." -ForegroundColor Green
    docker-compose --profile development up -d
    Write-Host "Geliştirme ortamı başlatıldı." -ForegroundColor Green
    Write-Host "Debug portu: localhost:4024" -ForegroundColor Yellow
}

function Stop-All {
    Write-Host "Tüm servisler durduruluyor..." -ForegroundColor Yellow
    docker-compose down
    Write-Host "Tüm servisler durduruldu." -ForegroundColor Green
}

function Restart-All {
    Write-Host "Tüm servisler yeniden başlatılıyor..." -ForegroundColor Yellow
    docker-compose restart
    Write-Host "Tüm servisler yeniden başlatıldı." -ForegroundColor Green
}

function Show-Logs {
    if ($Service -eq "") {
        docker-compose logs -f
    } else {
        docker-compose logs -f $Service
    }
}

function Build-App {
    Write-Host "Uygulama build ediliyor..." -ForegroundColor Green
    if ($NoCache) {
        docker-compose build --no-cache
    } else {
        docker-compose build
    }
    Write-Host "Build tamamlandı." -ForegroundColor Green
}

function Clean-All {
    Write-Host "Temizlik yapılıyor..." -ForegroundColor Yellow
    $response = Read-Host "Bu işlem tüm container'ları ve volume'ları silecek. Emin misiniz? (y/N)"
    if ($response -match "^[Yy]$") {
        docker-compose down -v --remove-orphans
        docker system prune -f
        Write-Host "Temizlik tamamlandı." -ForegroundColor Green
    } else {
        Write-Host "İşlem iptal edildi." -ForegroundColor Yellow
    }
}

function Show-Status {
    Write-Host "Servis durumları:" -ForegroundColor Cyan
    docker-compose ps
    Write-Host ""
    Write-Host "Container'lar:" -ForegroundColor Cyan
    docker ps -a --filter "name=hastaTakip"
}

function Initialize-Database {
    Write-Host "Veritabanı manuel olarak başlatılıyor..." -ForegroundColor Green
    docker-compose up -d sqlserver
    
    # Wait for SQL Server to be ready
    Write-Host "SQL Server'ın hazır olması bekleniyor..." -ForegroundColor Yellow
    Start-Sleep -Seconds 30
    
    # Run initialization script
    if (Test-Path "database/init.sql") {
        Write-Host "Veritabanı initialization script çalıştırılıyor..." -ForegroundColor Green
        docker exec -i hastaTakip-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourPassword123 -i /docker-entrypoint-initdb.d/init.sql
        Write-Host "Veritabanı başlatıldı." -ForegroundColor Green
    } else {
        Write-Warning "database/init.sql dosyası bulunamadı."
    }
}

function Switch-ToWindows {
    Write-Host "Windows containers moduna geçiliyor..." -ForegroundColor Green
    & "$Env:ProgramFiles\Docker\Docker\DockerCli.exe" -SwitchDaemon
    Write-Host "Windows containers moduna geçildi." -ForegroundColor Green
}

function Switch-ToLinux {
    Write-Host "Linux containers moduna geçiliyor..." -ForegroundColor Green
    & "$Env:ProgramFiles\Docker\Docker\DockerCli.exe" -SwitchLinuxEngine
    Write-Host "Linux containers moduna geçildi." -ForegroundColor Green
}

# Ana script logic
Test-Docker

switch ($Command) {
    "start-db" {
        Start-Database
    }
    "start-all" {
        Start-All
    }
    "start-dev" {
        Start-Development
    }
    "stop" {
        Stop-All
    }
    "restart" {
        Restart-All
    }
    "logs" {
        Show-Logs
    }
    "build" {
        Build-App
    }
    "clean" {
        Clean-All
    }
    "status" {
        Show-Status
    }
    "db-init" {
        Initialize-Database
    }
    "switch-windows" {
        Switch-ToWindows
    }
    "switch-linux" {
        Switch-ToLinux
    }
    "help" {
        Show-Help
    }
    default {
        Write-Error "Bilinmeyen komut: $Command"
        Write-Host "Yardım için: .\docker-helper.ps1 help" -ForegroundColor Yellow
        exit 1
    }
}