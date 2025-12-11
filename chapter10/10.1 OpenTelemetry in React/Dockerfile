FROM node:alpine AS build

WORKDIR /src

COPY package.json package.json
RUN npm install

COPY . .
# build .env
# prefix OTEL attributes with `VITE_` to pass through `vite build`
ARG VITE_OTEL_EXPORTER_OTLP_ENDPOINT
ARG VITE_OTEL_EXPORTER_OTLP_HEADERS
ARG VITE_OTEL_SERVICE_VERSION
ARG VITE_OTEL_SERVICE_NAME
RUN echo "VITE_OTEL_EXPORTER_OTLP_ENDPOINT=$VITE_OTEL_EXPORTER_OTLP_ENDPOINT" > .env
RUN echo "VITE_OTEL_EXPORTER_OTLP_HEADERS=$VITE_OTEL_EXPORTER_OTLP_HEADERS" >> .env
RUN echo "VITE_OTEL_SERVICE_VERSION=$VITE_OTEL_SERVICE_VERSION" >> .env
RUN echo "VITE_OTEL_SERVICE_NAME=$VITE_OTEL_SERVICE_NAME" >> .env

RUN npm run build
# ASSUME: it produces a dist folder


FROM nginx:alpine-otel

RUN echo "load_module modules/ngx_otel_module.so;" | cat - /etc/nginx/nginx.conf > /tmp/nginx.conf; mv /tmp/nginx.conf /etc/nginx/nginx.conf

COPY nginx.conf /etc/nginx/conf.d/default.conf

WORKDIR /usr/share/nginx/html
COPY --from=build /src/dist .

# nginx already has `CMD [...]`
