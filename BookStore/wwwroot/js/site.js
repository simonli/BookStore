function confirm_delete(url) {
    $.confirm({
        title: '⚠️ 警告',
        content: '此操作不可恢复，确认删除吗？',
        type: 'red',
        typeAnimated: true,
        buttons: {
            confirm: {
                text: '删除',
                btnClass: 'btn-danger',
                action: function () {
                    location.href = url;
                }
            },
            cancel: {
                text: '取消',
                btnClass: 'btn-default',
                action: function () {
                }
            }
        }
    });
}
