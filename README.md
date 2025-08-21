# Hasta Takip Sistemi (Patient Tracking System) - Docker Setup

Bu proje bir Windows Forms uygulaması olarak geliştirilmiş hasta takip sistemidir. Docker ile çalıştırabilmek için aşağıdaki talimatları takip edin.

## Gereksinimler

### Windows Containers için:
- Docker Desktop (Windows containers desteği aktif)
- Windows 10/11 veya Windows Server 2019+
- En az 4GB RAM

### Linux Containers için (Sadece Veritabanı):
- Docker ve Docker Compose
- Uygulama Windows Forms olduğu için GUI olmadan çalıştırılamaz

## Kurulum ve Çalıştırma

### 1. Projeyi Klonlayın
```bash
git clone <repository-url>
cd First
```

### 2. Docker Compose ile Çalıştırma

#### SQL Server'ı Başlatın (Linux/Windows):
```bash
# Sadece veritabanını başlatmak için
docker-compose up sqlserver -d
```

#### Tam Sistemi Başlatın (Windows Containers gerekli):
```bash
# Windows containers modunda
docker-compose up -d
```

### 3. Manuel Docker Build (Windows Containers)

```bash
# Windows containers moduna geçin
docker build -t hastaTakip-app .

# Uygulamayı çalıştırın
docker run -d --name hastaTakip-app \
  -e DB_CONNECTION_STRING="Data Source=sqlserver;Initial Catalog=HastaTakip;User Id=sa;Password=YourPassword123;Encrypt=False;" \
  hastaTakip-app
```

## Konfigürasyon

### Veritabanı Bağlantısı

Uygulama veritabanı bağlantı dizesini şu sırayla arar:

1. **Ortam Değişkeni**: `DB_CONNECTION_STRING`
2. **App.config**: `DefaultConnection` connection string
3. **Varsayılan**: Yerel SQL Server bağlantısı

#### Ortam Değişkeni ile Ayarlama:
```bash
# Windows
set DB_CONNECTION_STRING="Data Source=sqlserver;Initial Catalog=HastaTakip;User Id=sa;Password=YourPassword123;Encrypt=False;"

# Linux/Mac
export DB_CONNECTION_STRING="Data Source=sqlserver;Initial Catalog=HastaTakip;User Id=sa;Password=YourPassword123;Encrypt=False;"
```

## Veritabanı

### Otomatik Kurulum
- `database/init.sql` dosyası SQL Server container'ı başlatıldığında otomatik olarak çalışır
- Gerekli tablolar ve stored procedure'lar oluşturulur
- Varsayılan veriler eklenir

### Varsayılan Kullanıcı
- **Kullanıcı Adı**: admin
- **Şifre**: Admin123!

### Manuel Veritabanı Kurulumu
```sql
-- SQL Server'a bağlanın ve init.sql dosyasını çalıştırın
sqlcmd -S localhost,1433 -U sa -P YourPassword123 -i database/init.sql
```

## GUI Uygulaması Notları

Bu Windows Forms uygulaması olduğu için:

### Windows Containers ile Çalıştırma:
- Windows Server Core base image kullanır
- GUI erişimi için RDP veya benzer uzaktan erişim gerekir
- Container içinde GUI aplikasyonu çalışabilir ancak görüntüleme için ek konfigürasyon gerekir

### Alternatif Yaklaşımlar:
1. **RDP ile Erişim**: Container'a RDP bağlantısı kurarak GUI'ye erişim
2. **Console App'e Dönüştürme**: Uygulamayı konsol uygulamasına dönüştürme
3. **Web API'ye Dönüştürme**: REST API olarak yeniden tasarlama

## Port Bilgileri

- **SQL Server**: 1433
- **Uygulama**: GUI uygulaması olduğu için web portu yok

## Volume Bilgileri

- **sqlserver_data**: SQL Server veritabanı dosyaları
- **Uygulama**: Stateless çalışır, veri veritabanında saklanır

## Sorun Giderme

### 1. SQL Server Bağlantı Hatası
```bash
# SQL Server container'ının çalıştığını kontrol edin
docker ps

# Logları kontrol edin
docker logs hastaTakip-sqlserver
```

### 2. Windows Containers Hatası
```bash
# Docker Desktop'ta Windows containers moduna geçin
# PowerShell'de:
& $Env:ProgramFiles\Docker\Docker\DockerCli.exe -SwitchDaemon
```

### 3. Build Hatası
```bash
# Cache'i temizleyin
docker builder prune

# Yeniden build edin
docker-compose build --no-cache
```

## Geliştirme Ortamı

### Yerel Geliştirme:
1. SQL Server'ı Docker ile çalıştırın
2. Visual Studio ile uygulamayı geliştirin
3. Connection string'i localhost'a ayarlayın

```bash
# Sadece SQL Server'ı başlat
docker-compose up sqlserver -d
```

## Güvenlik Notları

- Üretim ortamında SA şifresini değiştirin
- Connection string'lerde güçlü şifreler kullanın
- Hassas bilgileri environment variables veya secrets ile yönetin

## Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun
3. Değişikliklerinizi commit edin
4. Pull request açın

## Lisans

Proje lisans bilgileri için repository'ye bakınız.