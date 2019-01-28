# httprequestdetails
An ASP.NET Core API that dumps all http headers and connection info related to an incoming request


# Enabled custom SSL certificate in kestrel
```
      "HttpsInlineCertFile": {
        "Url": "https://localhost:5005",
        "Certificate": {
          "Path": "",
          "Password": ""
        }
      },
      "HttpsDefaultCert": {
        "Url": "https://localhost:8443"
      }
```