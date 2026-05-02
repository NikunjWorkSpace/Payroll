# PayBridge Frontend

This Angular 17 app is structured so it can live in its own repository, independent of the `.NET` solution file and backend source tree.

## What makes it standalone

- its own [package.json](/c:/WorkSpace/Payroll/Payroll/Frontend/package.json)
- its own [angular.json](/c:/WorkSpace/Payroll/Payroll/Frontend/angular.json)
- its own [Dockerfile](/c:/WorkSpace/Payroll/Payroll/Frontend/Dockerfile)
- its own [.gitignore](/c:/WorkSpace/Payroll/Payroll/Frontend/.gitignore)
- its own [.dockerignore](/c:/WorkSpace/Payroll/Payroll/Frontend/.dockerignore)

If you move the full `Frontend/` folder into a new repository, it is already repo-root-ready.

## Run locally

```powershell
cd Frontend
npm install
npm start
```

The app expects the backend API at `http://localhost:8080/api`, configured in [environment.ts](/c:/WorkSpace/Payroll/Payroll/Frontend/src/environments/environment.ts).

## Run with Docker

```powershell
cd Frontend
docker compose up --build
```

This starts only the Angular frontend on `http://localhost:4200`.

## Notes for a separate repository

- publish `Frontend/` as a new Git repository root
- keep backend API hosted separately
- update [environment.ts](/c:/WorkSpace/Payroll/Payroll/Frontend/src/environments/environment.ts) if the API base URL changes
- if you want CI/CD next, add GitHub Actions for `npm install`, `ng build`, and container publish

