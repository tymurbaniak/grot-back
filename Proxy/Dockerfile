FROM nginx:stable-alpine AS build

COPY ./Proxy/certs/nginx-selfsigned.key /etc/ssl/private/nginx-selfsigned.key
COPY ./Proxy/certs/nginx-selfsigned.crt /etc/ssl/certs/nginx-selfsigned.crt
COPY ./Proxy/certs/dhparam.pem /etc/ssl/certs/dhparam.pem
COPY ./Proxy/nginx.conf /etc/nginx/conf.d/default.conf
COPY ./Proxy/self-signed.conf /etc/nginx/snippets/self-signed.conf
COPY ./Proxy/ssl-param.conf /etc/nginx/snippets/ssl-params.conf