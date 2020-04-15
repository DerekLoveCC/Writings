##使用Python调Restful API
>本文给出用GET方法从Restful API获取信息和用POST方法向Restful API发生消息。主要使用的包是urllib和json，其中urllib用来发送http请求，json包用来解析和构造json数据。具体例子如下：
###通过GET方法获取信息
```Python
import json
from urllib import request

query_url_addr='' #the Restful api url
query_headers={'cookie':'the cooke'} #the request headers
req = request.Request(query_url_addr, headers=query_headers)
resp = request.urlopen(req)
result = resp.read().decode()
result_json = json.loads(result)#the json object of response data
```
###用POST方法向Restful API发生消息
```Python
import json
import time
from urllib import request
from urllib import error
try:
    create_url=''#the create request url
    create_headers={'cookie':'the cooke'} #the request headers
    body_data_str='{"body":"bodytext"}'
    body_data = bytes(body_data_str, 'utf8')
    req = request.Request(create_url, headers=create_headers, data=body_data, method='POST')
    resp = request.urlopen(req)
    result = resp.read().decode()
    result_json = json.loads(result)
    return result_json
except error.HTTPError as err:
     error_body = err.file.read().decode()
     return  json.loads(result)
```