FROM node:20-alpine
WORKDIR /app
ENV HOST=0.0.0.0
ENV PORT=4200

COPY package*.json ./
RUN npm install

COPY . ./
EXPOSE 4200
CMD ["npm", "start", "--", "--host", "0.0.0.0"]
