

     var chart = c3.generate({
        bindto: '#range-y-axis',
        data: {
            columns: [
                ['Lợi nhuận', 30000, 20000, 10000, 40000, 0, 0, 0, 0, 0, 0, 0, 0]
            ],
            order:false,
            type: ''
          },
                bar: {
            width: {
                ratio: 0.70 
            }
            },
          axis: {
            x: {
            type: 'category',
            categories:["Tháng 1" , "Tháng 2","Tháng 3","Tháng 4","Tháng 5","Tháng 6","Tháng 7","Tháng 8","Tháng 9","Tháng 10","Tháng 11","Tháng 12"],
              tick: {
                rotate: 30,
                format: '%b%Y'
              }
            }
          }
      });