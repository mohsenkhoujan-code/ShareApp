import mimetypes,random
from re import S
from click import Path
from django.http import HttpResponse,JsonResponse
from django.shortcuts import render
from django.contrib.auth import authenticate,logout
from shiboken6 import isValid
import os

from TeamWork import settings
from .models import *
from .forms import *

from django.middleware.csrf import get_token
from django.views.decorators.csrf import csrf_exempt

def csrf_view(request):
    return JsonResponse({'csrftoken': get_token(request)})

# Create your views here.
def recieve(req):
    username = req.GET.get('username')
    password = req.GET.get('password')
    content = req.GET.get('content')
    user = authenticate(username=username,password=password)

    if user != None:
        db_chat.objects.create(
            username=username,
            text=content
        )
        settings.DATABASE_CHANGE_EVENT_CHAT = True
        logout(req)
    else:
        db_chat.objects.create(
            username=username,
            text="User object is none"
        )
    return render(req,"index.html")

def refresh(req):
    username = req.GET.get('username')
    password = req.GET.get('password')
    user = authenticate(username=username,password=password)

    if user != None:
        json_stat = []
        json_stat_none = []
        logout(req)
        if(db_chat.objects.all().exists()):
            chats = db_chat.objects.order_by('id')
            for chat in chats:
                json = {}
                json['id'] = chat.id
                json['username'] = chat.username
                json['datetime'] = chat.datetime.ctime()
                json['content'] = chat.text
                json_stat.append(json)
            
        else:
            json_stat_none.append({'item':'None'})
            return JsonResponse({'json_stat_none':json_stat_none})
        return JsonResponse({'json_stat':json_stat})
        
        
    else: return JsonResponse({"item":"UnR"})

def login(req,getOrPost):
    username = getOrPost.get('username')
    password = getOrPost.get('password')
    user = authenticate(username=username,password=password)
    if user != None:
        logout(req)
        return True
    else:
        return False

@csrf_exempt
def upload_file(request):
    if request.method == "POST":
        print(request.FILES)
        username = request.POST.get("username")
        caption = request.POST.get("caption")
        file = request.FILES.get("file")
        format_ = request.POST.get("format")
        if login(request,request.POST):
            
            while(True):
                randIntId = random.randint(0,19999999999999999999999999999)
                if randIntId not in settings.ALL_FETCHES_FILES:
                    url = f"{settings.MEDIA_ROOT}/uploads/{file.name}-{username}-{randIntId}{format_}"
                    db_files.objects.create(
                        username=username,
                        caption=caption,
                        file=url
                    )
                    with open(url,'wb')as file_:
                        file_.write(file.read())
                    break
                    #with open(settings.MEDIA_ROOT+"/uploads/",'wb')as file:

            settings.DATABASE_CHANGE_EVENT = True
            return JsonResponse({'code':'200','message':'success'})
        else:
            return JsonResponse({'code':'401','message':'fail to authenticating'})
    return JsonResponse({'code':'700','message':'none'})

def get_contentFiles(req):
    if login(req,req.GET):
        JsonFiles = []
        all = db_files.objects.order_by('id')
        for row in all:
            JsonFiles.append(
                {
                    'username':row.username,
                    'caption':row.caption,
                    'datetime':row.datetime.ctime(),
                    'id':row.id
                }
            )
        return JsonResponse({'content':JsonFiles})
    else:
        return JsonResponse({'code':'401','message':'fail to authenticating'})


def download(req,username,password,id):
    if login(req,{'username':username,'password':password}):
        file = db_files.objects.get(id=id)
        with open(file.file,'rb') as file_:
            response = HttpResponse(file_.read(),content_type=mimetypes.guess_type(file.file))
            response['Content-Disposition'] = f"attachment; filename={os.path.basename(file.file)}"
            response['Content-Length'] = str(os.path.getsize(file.file))
            response['File-name'] = os.path.basename(file.file)
            response['User-caption'] = file.caption
            response['User-name'] = file.username
            response['File-datetime'] = file.datetime.ctime()

            return response
    return HttpResponse("Fail authenticate",status=403)

def ChangeEvent(req):
    if login(req,req.GET):
        if settings.DATABASE_CHANGE_EVENT:
            if settings.DATABASE_CHANGE_EVENT_COUNT <= 3:
                settings.DATABASE_CHANGE_EVENT_COUNT+=1
            else:
                settings.DATABASE_CHANGE_EVENT_COUNT=0
                settings.DATABASE_CHANGE_EVENT=False
                print("--- - -- - - - -- < < < 1929231")
            return HttpResponse("1929231")
        return HttpResponse("123323")
    return HttpResponse("Authenticated fail")

def ChangeEventChat(req):
    if login(req,req.GET):
        if settings.DATABASE_CHANGE_EVENT_CHAT:
            if settings.DATABASE_CHANGE_EVENT_COUNT_CHAT < 3:
                settings.DATABASE_CHANGE_EVENT_COUNT_CHAT+=1
                print("--- - -- - - - -- < < < 1929231 on")
            else:
                settings.DATABASE_CHANGE_EVENT_COUNT_CHAT=0
                settings.DATABASE_CHANGE_EVENT_CHAT=False
                print("--- - -- - - - -- < < < 1929231")
            return HttpResponse("1929231")
        return HttpResponse("123323")
    return HttpResponse("Authenticated fail")
def ck(req):
    return HttpResponse("Be great")

def DeleteFile(req):
    if login(req,req.GET):
        RemoveId = int(req.GET.get("id"))
        ipd = db_files.objects.get(id=int(RemoveId))
        os.remove(ipd.file)
        ipd.delete()
        settings.DATABASE_CHANGE_EVENT = True
        
        return HttpResponse("1929231")
    return HttpResponse("123323")
