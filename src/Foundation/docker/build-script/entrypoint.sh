#!/bin/bash

# Start the script to create the DB and user
/usr/config/setup-databases.sh &

# Start SQL server
/opt/mssql/bin/sqlservr
