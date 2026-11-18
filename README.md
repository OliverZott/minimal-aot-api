# minimal-aot-api

## Run / Publish

```bash
dotnet run
dotnet publish -c Release
```

## todo

- use ; delimiter in csvexport
- https
- CI to publish win/linux libs
- Documentation for API usage
- hardening tip: the Server: Kestrel header tells attackers what you run, and it can be switched off

## Create mariadb user for read only

- `mariadb -u root -p`
- `SELECT VERSION();`
- `CREATE USER 'csv_export'@'localhost' IDENTIFIED BY '<password>';`
- `GRANT SELECT ON sglt2.* TO 'csv_export'@'localhost';`
- `FLUSH PRIVILEGES;`

```bash
SELECT User, Host
FROM mysql.user
ORDER BY User;

SHOW GRANTS FOR 'csv_export'@'localhost';
```
