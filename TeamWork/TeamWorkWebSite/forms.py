from django import forms
from .models import db_files

class FormUpload(forms.ModelForm):
    class meta:
        model = db_files()
        fields = ['username','caption','file']