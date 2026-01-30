# ============================
# Stage 1 — Build Angular app
# ============================
FROM node:20 AS build
WORKDIR /app

# Install dependencies
COPY package.json package-lock.json ./
RUN npm ci

# Copy app source
COPY . .

# Build Angular for production
RUN npm run build -- --configuration production


# ============================
# Stage 2 — Run with Nginx
# ============================
FROM nginx:alpine

# Remove default nginx content
RUN rm -rf /usr/share/nginx/html/*

# Copy Angular build output
# Angular 17+ outputs to dist/frontend/browser
COPY --from=build /app/dist/frontend/browser /usr/share/nginx/html

# Fix Angular CSR file name if needed
RUN if [ -f /usr/share/nginx/html/index.csr.html ] && [ ! -f /usr/share/nginx/html/index.html ]; then \
        mv /usr/share/nginx/html/index.csr.html /usr/share/nginx/html/index.html; \
    fi

# Copy nginx config template
COPY nginx.conf /etc/nginx/templates/default.conf.template

# Expose nginx
EXPOSE 80

# Inject Railway PORT and start nginx
CMD ["sh", "-c", "envsubst '$PORT' < /etc/nginx/templates/default.conf.template > /etc/nginx/conf.d/default.conf && nginx -g 'daemon off;'"]
