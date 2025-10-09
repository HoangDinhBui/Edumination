# ============ build ============
FROM node:22-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
RUN npm install lucide-react
COPY . .
# VITE_BASE_URL có thể cần nếu deploy dưới subpath
RUN npm run build

# ============ runtime ============
FROM nginx:alpine
# cấu hình SPA fallback
COPY ./nginx-frontend.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
