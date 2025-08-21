#!/bin/bash

# Docker management script for Hasta Takip Sistemi
# Bu script Docker işlemlerini kolaylaştırmak için hazırlanmıştır

set -e

COMPOSE_FILE="docker-compose.yml"
PROJECT_NAME="hastaTakip"

show_help() {
    echo "Hasta Takip Sistemi Docker Yönetim Scripti"
    echo ""
    echo "Kullanım: $0 [KOMUT] [SEÇENEKLER]"
    echo ""
    echo "Komutlar:"
    echo "  start-db        Sadece SQL Server'ı başlat"
    echo "  start-all       Tüm servisleri başlat (Windows containers gerekli)"
    echo "  start-dev       Geliştirme ortamını başlat"
    echo "  stop            Tüm servisleri durdur"
    echo "  restart         Tüm servisleri yeniden başlat"
    echo "  logs            Logları göster"
    echo "  build           Uygulamayı yeniden build et"
    echo "  clean           Tüm container ve volume'ları temizle"
    echo "  status          Servis durumlarını göster"
    echo "  db-init         Veritabanını manuel olarak başlat"
    echo "  help            Bu yardım mesajını göster"
    echo ""
    echo "Örnekler:"
    echo "  $0 start-db                    # Sadece SQL Server'ı başlat"
    echo "  $0 start-all                   # Tüm sistemi başlat"
    echo "  $0 logs hastaTakip-sqlserver   # SQL Server loglarını göster"
    echo "  $0 build --no-cache            # Cache kullanmadan build et"
}

check_docker() {
    if ! command -v docker &> /dev/null; then
        echo "Hata: Docker kurulu değil!"
        exit 1
    fi
    
    if ! command -v docker-compose &> /dev/null; then
        echo "Hata: Docker Compose kurulu değil!"
        exit 1
    fi
}

start_database() {
    echo "SQL Server başlatılıyor..."
    docker-compose up -d sqlserver
    echo "SQL Server başlatıldı. Bağlantı: localhost:1433"
    echo "SA Şifresi: YourPassword123"
}

start_all() {
    echo "Tüm servisler başlatılıyor (Windows containers gerekli)..."
    docker-compose --profile production up -d
    echo "Tüm servisler başlatıldı."
}

start_development() {
    echo "Geliştirme ortamı başlatılıyor..."
    docker-compose --profile development up -d
    echo "Geliştirme ortamı başlatıldı."
    echo "Debug portu: localhost:4024"
}

stop_all() {
    echo "Tüm servisler durduruluyor..."
    docker-compose down
    echo "Tüm servisler durduruldu."
}

restart_all() {
    echo "Tüm servisler yeniden başlatılıyor..."
    docker-compose restart
    echo "Tüm servisler yeniden başlatıldı."
}

show_logs() {
    if [ -z "$2" ]; then
        docker-compose logs -f
    else
        docker-compose logs -f "$2"
    fi
}

build_app() {
    echo "Uygulama build ediliyor..."
    if [ "$2" = "--no-cache" ]; then
        docker-compose build --no-cache
    else
        docker-compose build
    fi
    echo "Build tamamlandı."
}

clean_all() {
    echo "Temizlik yapılıyor..."
    read -p "Bu işlem tüm container'ları ve volume'ları silecek. Emin misiniz? (y/N): " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        docker-compose down -v --remove-orphans
        docker system prune -f
        echo "Temizlik tamamlandı."
    else
        echo "İşlem iptal edildi."
    fi
}

show_status() {
    echo "Servis durumları:"
    docker-compose ps
    echo ""
    echo "Container'lar:"
    docker ps -a --filter "name=hastaTakip"
}

init_database() {
    echo "Veritabanı manuel olarak başlatılıyor..."
    docker-compose up -d sqlserver
    
    # Wait for SQL Server to be ready
    echo "SQL Server'ın hazır olması bekleniyor..."
    sleep 30
    
    # Run initialization script
    if [ -f "database/init.sql" ]; then
        echo "Veritabanı initialization script çalıştırılıyor..."
        docker exec -i hastaTakip-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourPassword123 -i /docker-entrypoint-initdb.d/init.sql
        echo "Veritabanı başlatıldı."
    else
        echo "Uyarı: database/init.sql dosyası bulunamadı."
    fi
}

# Ana script logic
check_docker

case "${1:-help}" in
    "start-db")
        start_database
        ;;
    "start-all")
        start_all
        ;;
    "start-dev")
        start_development
        ;;
    "stop")
        stop_all
        ;;
    "restart")
        restart_all
        ;;
    "logs")
        show_logs "$@"
        ;;
    "build")
        build_app "$@"
        ;;
    "clean")
        clean_all
        ;;
    "status")
        show_status
        ;;
    "db-init")
        init_database
        ;;
    "help"|"--help"|"-h")
        show_help
        ;;
    *)
        echo "Bilinmeyen komut: $1"
        echo "Yardım için: $0 help"
        exit 1
        ;;
esac