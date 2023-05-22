#This is a sample Image 
FROM ubuntu 

RUN apt-get update 
RUN apt-get install –y nginx 
CMD [“echo”,”Image created”] 

#documentation(default for nginx is 80)
# EXPOSE 80
