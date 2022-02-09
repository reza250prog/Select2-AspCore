$("#ajaxSelect2").select2({
    placeholder: "Select a Value from API Data",
    allowClear: true,
    language: "fa",
    ajax: {
        url: "/api/customer/search",
        delay: 250,
        contentType: "application/json; charset=utf-8",
        data: function (params) {
            var query =
            {
                term: params.term,
                page: params.page || 1
            };
            return query;
        },
        processResults: function (data, params) {
            console.log(params);
            console.log(data);
            return {
                results: data.items,
                page: params.page,
                pagination: {
                    more: data.more
                }
            }
        }
    }
});

