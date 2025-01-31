#!/bin/bash

export cms_db=${cms_db}
export commerce_db=${commerce_db}
#export user=${user}
#export password=${password}  # Added quotes for special characters

export sql="/opt/mssql-tools18/bin/sqlcmd -S . -U sa -P ${MSSQL_SA_PASSWORD} -C -Y 30"

# Improved database exists check
database_exists() {
    local db_name=$1
    local exists=$($sql -Q "SET NOCOUNT ON; SELECT CAST(COUNT(*) AS BIT) FROM sys.databases WHERE name = '$db_name'" -W -h-1)
    [ "$exists" -eq "1" ]
}

echo "@Waiting for MSSQL server to start..."
export STATUS=1
i=0

# Fixed counter increment
while [[ $STATUS -ne 0 ]] && [[ $i -lt 12 ]]; do  # 60 iterations → 12 (60/5)
    sleep 5s
    ((i++))
    $sql -Q "SELECT 1" > /dev/null 2>&1
    STATUS=$?
    echo "Attempt $i/12: Checking server status..."
done

if [ $STATUS -ne 0 ]; then 
    echo "Error: MSSQL server failed to start within 1 minute"
    exit 1
fi

# Database creation function
create_db() {
    local db_name=$1
    echo "Creating $db_name database..."
    $sql -Q "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '$db_name') 
             CREATE DATABASE [$db_name] COLLATE SQL_Latin1_General_CP1_CI_AS"
}

create_db "$cms_db"
create_db "$commerce_db"

# Improved login check
# check_login_exists() {
#    local username=$1
#    $sql -Q "SET NOCOUNT ON; SELECT CAST(COUNT(*) AS BIT) FROM master.sys.server_principals WHERE name = '$username'" -W -h-1
#}

#user_exists=$(check_login_exists "$user")

# User management
#if [ "$user_exists" -eq "1" ]; then
#    echo "Updating existing user..."
#    $sql -Q "ALTER LOGIN [$user] WITH PASSWORD = '$password', CHECK_POLICY = OFF"
#else
#    echo "Creating new user..."
#    $sql -Q "CREATE LOGIN [$user] WITH PASSWORD = '$password', DEFAULT_DATABASE = [$cms_db], CHECK_POLICY = OFF"
#fi

# Database permissions setup
#setup_permissions() {
#    local db=$1
#    echo "Configuring $db permissions..."
#    $sql -d "$db" -Q "IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = '$user')
#                      CREATE USER [$user] FOR LOGIN [$user];
#                      ALTER ROLE [db_owner] ADD MEMBER [$user];"
#}

#setup_permissions "$cms_db"
#setup_permissions "$commerce_db"

# SQL script execution
echo "Processing initialization scripts..."
find /docker-entrypoint-initdb.d/ -type f -name "*.sql" | while read -r f; do
    echo "Executing $f..."
    $sql -d "$commerce_db" -b -i "$f" || echo "Error executing $f"
done

echo "Database initialization completed successfully."