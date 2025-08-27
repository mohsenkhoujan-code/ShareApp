from django.urls import path,include
from django.conf.urls.static import static

from TeamWork import settings
from .views import *

urlpatterns = [
    path("ClientR0/Actions/pa/recieve",recieve),
    path("ClientR0/Actions/pa/refresh",refresh),
    path("ClientR0/CSRF_PROTECTION/x",csrf_view),
    path("ClientR0/conncetions/checkServerConnection10108000/",ck),
    path("ClientR0/Actions/fl/Upload",upload_file),
    path("ClientR0/Actions/fl/Explore",get_contentFiles),
    path("ClientR0/Actions/fl/DATABASE_EVENTS",ChangeEvent),
    path("ClientR0/Actions/fl/DATABASE_EVENTS_CHAT",ChangeEventChat),
    path("ClientR0/Actions/fl/Delete",DeleteFile),
    path("ClientR0/Actions/fl/Download/<str:username>/<str:password>/<int:id>",download),

]
urlpatterns += static(settings.MEDIA_URL, document_root=settings.MEDIA_ROOT)