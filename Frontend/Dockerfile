# ================= Development Stage =================
FROM node:22-alpine AS dev

WORKDIR /app

COPY package*.json ./
RUN npm install

# Hot reload config (Vite/React/Vue)
ENV CHOKIDAR_USEPOLLING=true
ENV HOST=0.0.0.0

EXPOSE 5173

# Mặc định chạy "npm run dev" trong stage dev
CMD ["npm", "run", "dev"]


# ================= Production Build Stage =================
FROM node:22-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build


# ================= Nginx Serve Stage =================
FROM nginx:alpine AS prod
COPY ./nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
