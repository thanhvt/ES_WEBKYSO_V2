(function (ngocben) {
    "use strict";
    $(document).ready(function () {
        ngocben(jQuery);
    });
})(function ($) {


    window.ObjectContaintKey = function (obj, val) {
        // val in obj
        var r = false;
        $.each(obj, function (name, value) {
            if (val.toLowerCase() === name.toLowerCase()) {
                r = name;
            }
        });
        return r;
    }

    window.ObjectToString = function (obj, reg1, reg2) {
        if (typeof obj !== "object") {
            return;
        }

        var retStr = "";

        $.each(obj, function (name, value) {
            retStr += name + (reg1 || "=") + value + (reg2 || "&");
        });
        var unplus = retStr.length - 1;
        if (reg2) {
            unplus = retStr.length - reg2.length;
        }
        retStr = retStr.substring(0, unplus);
        return retStr;
    }

    window.URLParameter = function () {
        var sPageUrl = decodeURIComponent(window.location.search.substring(1));
        var sUrlVariables = sPageUrl.split('&');
        var retObj = {};

        $.each(sUrlVariables, function (index, data) {
            var pData = data.split("=");
            if (pData.length > 1 && pData[1] !== "" && window.ObjectContaintKey(retObj, pData[0]) === false) {
                retObj[pData[0]] = pData[1];
            }

        });

        return retObj;
    }

    window.GetURLParameter = function (sParam) {
        var sPageUrl = URLParameter();
        var key = "";
        $.each(sPageUrl, function (name, value) {

            if (sParam.toLowerCase() === name.toLowerCase()) {
                key = name;
            }

        });
        if (key !== "") {
            return sPageUrl[key];
        }
        return null;
    }

    window.guid = function () {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
              .toString(16)
              .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
          s4() + '-' + s4() + s4() + s4();
    }

    window.modalAlert = function (db) {
        $("#staticModal").html('<div class="modal-body"><div>' + db || "Đang xử lý dữ liệu..." + '</div></div>');
        $('#staticModal').modal('show');
    }

    window.modalAlert.close = function () {
        $('#staticModal').modal('hide');
    }

    window.formatNumber = function (nStr) {
        if (nStr === undefined || nStr === null) {
            return "";
        }
        nStr = nStr.toString();
        nStr += '';
        nStr = nStr.replace(/,/g, '');
        x = nStr.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
    }

    window.toTitleCase = function (str) {
        return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
    }


    var lacTroi = function (params) {

        if (typeof params !== "object") {
            return;
        }

        var state = {
            "canBeAnything": true
        };

        var reUrl = "";

        var objUrl = URLParameter();


        $.each(params, function (name, value) {
            var gKey = window.ObjectContaintKey(objUrl, name);
            if (gKey !== false) {
                objUrl[gKey] = value;
                if (value === "") {
                    delete objUrl[gKey];
                }
            } else {
                objUrl[name] = value;
                if (value === "") {
                    delete objUrl[name];
                }
            }
        });

        reUrl = ObjectToString(objUrl);

        history.pushState(state, "", reUrl === "" ? window.location.pathname : ("?" + reUrl));

    }

    var nbPro = function (tableConfig) {

        if (tableConfig.id !== undefined) {

            if ($.fn.DataTable.isDataTable('#' + tableConfig.id)) {
                $('#' + tableConfig.id).dataTable().fnDestroy();
                $('#' + tableConfig.id).dataTable().fnClearTable();
                $('#' + tableConfig.id).dataTable().fnDestroy();
                $('#' + tableConfig.id).removeAttr('role');
                $('#' + tableConfig.id).removeAttr('aria-describedby');
                $('#' + tableConfig.id).html('<thead></thead><tbody></tbody>');
            }

            var columns = [];

            if (tableConfig.columns !== undefined) {
                if (typeof tableConfig.columns === "string") {
                    tableConfig.columns = tableConfig.columns.split(',');
                    $.each(tableConfig.columns,
                        function (i, data) {
                            columns.push({
                                "searchable": false,
                                "orderable": false,
                                "data": data
                            });
                        });
                } else {
                    columns = tableConfig.columns;
                }
            }


            $.each(columns, function (i, data) {
                if (data.skip !== undefined && data.skip === true) {

                } else {
                    var columnName = data.data;
                    if (data.header !== undefined) {
                        columnName = data.header;
                    }
                    if ($('#' + tableConfig.id + " thead tr").length === 0) {
                        $('#' + tableConfig.id + " thead").append($('<tr></tr>'));
                    }
                    if (data.type !== undefined && data.type === "checkbox") {
                        $('#' + tableConfig.id + " thead tr").append(
                            $('<th class="center"><div class="checkbox-table"><label><input type="checkbox" value="" class="grey group-checkable">' + (data.header === undefined ? "" : columnName) + '</label></div></th>')
                        );
                    } else {
                        $('#' + tableConfig.id + " thead tr").append($('<th>' + (columnName === null ? "" : columnName) + '</th>'));
                    }

                }
            });

            var defSearch = {
                "sSearch": GetURLParameter(tableConfig.id + "Search") === null ? "" : decodeURIComponent(GetURLParameter(tableConfig.id + "Search"))
            };
            var defLength = GetURLParameter(tableConfig.id + "Length");
            var defPage = GetURLParameter(tableConfig.id + "Page");

            var defStart = defPage ? eval((defLength || 10) + "*(" + defPage + " - 1)") : 0;
            //Thêm chức năng xử lý sau cùng
            var tbaleConfig = {
                "processing": true,
                "serverSide": true,
                "autoWidth": false,
                ajax: {
                    "type": "POST",
                    "url": tableConfig.url || "/JsonNotFound",
                    "data": tableConfig.reloadFunc,
                    "complete": function(sdbn) {
                        if (tableConfig.ajaxComplete) tableConfig.ajaxComplete(sdbn);
                    }
                },
                select: tableConfig.select || [],
                //"columnDefs": tableConfig.columnDefs || [],
                "search": {
                    value: "87887"
                },
                "columns": columns,
                "oSearch": defSearch,
                "pageLength": defLength || 10,
                "displayStart": defStart,
                "oLanguage": {
                    "sLengthMenu": "Hiển thị _MENU_ dòng mỗi trang.",
                    "sZeroRecords": "Không có dữ liệu.",
                    "sInfo": "Bản ghi từ _START_ đến _END_ trong tổng số _TOTAL_",
                    "sInfoEmpty": "",
                    "sSearch": "",
                    "oPaginate": {
                        "sPrevious": "",
                        "sNext": ""
                    },
                    "aaSorting": [
                        //[1, 'asc']
                    ]
                },
                "order": [
                    [0, "asc"]
                ],
                "aLengthMenu": [
                    [10, 15, 20, -1],
                    [10, 15, 20, "Tất cả"] // change per page values here
                ]

            };

            if (tableConfig.AddOn && typeof tableConfig === "object") {
                $.each(tableConfig.AddOn, function (key, val) {
                    tbaleConfig[key] = val;
                });
            }


            var gridPro = $('#' + tableConfig.id)
                .on('xhr.dt', function (e, settings, json, xhr) {
                    //console.log(e);
                    //console.log(settings);
                    //console.log(json);
                    //console.log(xhr);
                })
                .DataTable(tbaleConfig);
            $('#' + tableConfig.id + '_wrapper .dataTables_filter input').addClass("form-control input-sm").attr("placeholder", "Tìm kiếm");
            // modify table search input
            $('#' + tableConfig.id + '_wrapper .dataTables_length select').addClass("small-choice");
            // modify table per page dropdown
            $('#' + tableConfig.id + '_wrapper .dataTables_length select').select2();

            if (tableConfig.func && typeof tableConfig.func === "function") {
                tableConfig.func(gridPro);

            }

            gridPro.iCheckHelp = function () {
                var allCheck = true;
                $.each($('#' + tableConfig.id + ' input.checkboxes'), function (i, data) {
                    if (!$(data).is(':checked')) {
                        allCheck = false;
                    }
                });
                if (allCheck) {
                    $('#' + tableConfig.id + ' input.group-checkable').iCheck('check');
                } else {
                    $('#' + tableConfig.id + ' input.group-checkable').iCheck('uncheck');
                }
            };

            gridPro.iCheckAllRunning = false;
            gridPro.iCheckChildRunning = false;

            gridPro.on('draw.dt', function () {

                //// ##### on donald trump 

                var infoPage = gridPro.page.info();

                var searchConfig = {};
                searchConfig[tableConfig.id + "Search"] = gridPro.search();

                var pageConfig = {};

                if (infoPage.page !== 0) {
                    pageConfig[tableConfig.id + "Page"] = (infoPage.page + 1);
                } else {
                    pageConfig[tableConfig.id + "Page"] = "";
                }

                var lengthConfig = {};

                if (infoPage.length !== 10) {
                    lengthConfig[tableConfig.id + "Length"] = (infoPage.length);
                } else {
                    lengthConfig[tableConfig.id + "Length"] = "";
                }

                lacTroi(searchConfig);
                lacTroi(pageConfig);
                lacTroi(lengthConfig);

                //// ##### on donald trump 


                window.icheckInit();

                $('#' + tableConfig.id + ' .centerpr').parent().addClass("center");
                $('#' + tableConfig.id + ' input.group-checkable').iCheck('uncheck');
                gridPro.iCheckHelp();

                $('#' + tableConfig.id + " input.checkboxes").iCheck({
                    checkboxClass: 'icheckbox_minimal-grey',
                    radioClass: 'iradio_minimal-grey',
                    increaseArea: '10%'
                });

                $('#' + tableConfig.id + " input.checkboxes").on('ifChanged', function () {
                    if (!gridPro.iCheckAllRunning) {
                        gridPro.iCheckChildRunning = true;
                        if ($(this).attr("edited") === undefined) {
                            $(this).attr("edited", "");
                        } else {
                            $(this).removeAttr("edited");
                        }
                        gridPro.iCheckHelp();
                        gridPro.iCheckChildRunning = false;
                    }
                });

                $('#' + tableConfig.id + " input.group-checkable").on('ifChanged', function () {
                    if (!gridPro.iCheckChildRunning) {
                        gridPro.iCheckAllRunning = true;
                        $.each($('#' + tableConfig.id + ' input.checkboxes'),
                                function (i, data) {
                                    var stateIf = $('#' +
                                            tableConfig
                                            .id +
                                            " input.group-checkable")
                                        .is(':checked');

                                    if ($(data).is(':checked') !== stateIf) {
                                        if ($(data).attr("edited") === undefined) {
                                            $(data).attr("edited", "");
                                        } else {
                                            $(data).removeAttr("edited");
                                        }
                                        $(data)
                                            .iCheck($('#' + tableConfig.id + " input.group-checkable").is(':checked') ? 'check' : 'uncheck');
                                    }

                                    //$(data)
                                    //    .iCheck($('#' + _tableConfig.id + " input.group-checkable").is(':checked') ? 'check' : 'uncheck');
                                })
                            .promise()
                            .done(function () {
                                gridPro.iCheckAllRunning = false;
                            });
                    }
                });

                if (tableConfig.funcOnDraw && typeof tableConfig.funcOnDraw === "function") {
                    tableConfig.funcOnDraw();
                }

            });



        }
    }

    window.AutoDataTable = function () {
        if (window.TableConfig !== undefined && window.TableConfig.length > 0) {
            $.each(window.TableConfig, function (i, data) {
                nbPro(data);
            });
            //https://www.youtube.com/watch?v=LCIhdcxun8o
        }
    }
    AutoDataTable();

})