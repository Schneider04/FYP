FROM nginx:1.20.1-alpine
COPY src/html /usr/share/nginx/html

#documentation(default for nginx is 80)
# EXPOSE 80
