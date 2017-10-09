function showQuestion(john) {                   //json的数据类型有的是从QuestionModel 过来的  Q_Num Q_Title Q_Description 
    var left1 = document.getElementById('content')
    left1.innerHTML = ''
    var json = JSON.parse(john)
    var left = document.getElementById('content')
    var form = document.createElement('form')
    form.id = 'postForm'
    for (var i = 0; i < json.length; i++) {
        var div = document.createElement('div')
        var div2 = document.createElement('div')
        var ID = document.createElement('p')
        var title = document.createElement('p')
        var description = document.createElement('p')

        var Q_ID = json[i].Q_Num
        ID.innerHTML = Q_ID
        title.innerHTML = json[i].Q_Title
        description.innerHTML = json[i].Q_Description

        div.style.cssText = 'width:500px;height:100px;margin-left:200px;margin-top:20px;box-shadow: 2px 2px 5px 2px rgb(192, 192, 192);border-radius: 3px; background-color :rgb(192, 192, 192);'
        div.addEventListener('click', function (ID) {
            return function () {
                var form = document.getElementById('postForm');
                form.action = '../Search/QuestionDetails?Q_ID=' + ID;
                form.method = 'post';
                form.submit();
            };
        }(Q_ID))
        div2.style.cssText = 'float:left;width:100px;height:100px;border-right:1px solid #000000'
        ID.style.cssText = 'margin-left:40px;padding-top:20px;'
        title.style.cssText = 'float:top;padding-top:20px;'
        description.style.cssText = 'float:top;margin-top:20px;'

        div2.appendChild(ID)
        div.appendChild(div2)
        div.appendChild(title)
        div.appendChild(description)
        form.appendChild(div)
    }
    left.appendChild(form)

}

function submitFunction(x)
{
    return function ()
    {
        alert(x)
        var form = document.getElementById('postForm')
        form.action = '../Home/test?Q_ID=' + x
        form.method = 'post'
        form.submit()
    }

}

function navigation(json)
{
    var nav = document.getElementsByClassName('nav3')[0]
    for(var i=0;i<json.length;i++)
    {
        var li = document.createElement('li')
        var a = document.createElement('a')
        var input = document.createElement('input')
        input.type = 'button'
        input.className = 'btn2'
        input.value = json[i].fieldname
        a.href = '../Search/FieldQuestion?field=' + json[i].fieldname
        a.appendChild(input)
        li.appendChild(a)
        nav.appendChild(li)
    }

}