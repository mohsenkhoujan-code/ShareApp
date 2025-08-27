from email.policy import default
from django.db import models

# Create your models here.
class db_chat(models.Model):
    id=models.IntegerField(primary_key=True)
    username = models.CharField(max_length=255)
    datetime = models.DateTimeField(auto_now_add=True)
    text = models.TextField(null=True)
    def __str__(self):
        return f"{self.username} - {self.datetime}"

class db_encCount(models.Model):
    username=models.CharField(max_length=255)
    countKey=models.IntegerField()
    def __str__(self):
        return f"{self.username} :-> {self.countKey}"

class db_files(models.Model):
    id=models.IntegerField(primary_key=True)
    username = models.CharField(max_length=255)
    datetime = models.DateTimeField(auto_now_add=True)
    caption = models.CharField(max_length=255)
    file = models.TextField()
    def __str__(self):
        return f"{self.caption} - {self.username} - {self.datetime.ctime()}"

