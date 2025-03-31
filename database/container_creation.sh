podman pod create --name nuvo-workout -p 9875:80 -p 5432:5432

podman run --pod=nuvo-workout \
    -e 'PGADMIN_DEFAULT_EMAIL=user@domain.com' \
    -e 'PGADMIN_DEFAULT_PASSWORD=SuperSecret' \
    --name pgadmin4 \
    -d dpage/pgadmin4:latest

podman run --pod=nuvo-workout \
    -e POSTGRES_USER=nuvo_workout \
    -e POSTGRES_PASSWORD=SuperSecret \
    --name db \
    -d postgres:latest