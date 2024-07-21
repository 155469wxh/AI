private void cbType_SelectedValueChanged(object sender, EventArgs e)
{
    //这一行代码获取了下拉框 cbType 中当前选定的文本值，
    //并将其存储在名为 Type 的字符串变量中。
    string Type = cbType.Text.ToString();
    //这一行代码声明了一个名为 TypeUnit 的字符串变量，并将其初始化为空字符串。
    string TypeUnit = "";
    //这一行代码创建了一个名为 dtDate 的 DataTable 对象。
    DataTable dtDate = new DataTable();

    //这是一个条件语句，检查 Type 是否等于字符串 "铁水废钢电炉冶炼"。
    if (Type == "铁水废钢电炉冶炼")
    {
        //如果 Type 等于 "铁水废钢电炉冶炼"，
        //则调用 melting.GetMEC_HM_Calculate("MEC_HM") 方法，
        //并将返回的 DataTable 对象赋给 dtDate 变量。
        dtDate = melting.GetMEC_HM_Calculate("MEC_HM");
        //这一行代码将 TypeUnit 变量设置为 "%"。
        //因为你之前提到了铁水废钢电炉冶炼，这里的单位很可能是百分比（%）
        TypeUnit = "%";
    }
    //这一行代码将 TypeUnit 的值设置为 lbUnit 标签的文本内容。
    // lbUnit 似乎是一个 UI 控件，可能是一个标签（Label）或者其他文本显示控件。
    //这行代码的作用是将 % 这个单位显示在用户界面上。
    //这个是掌管“请输入计算数值_____%的”  
    lbUnit.Text = TypeUnit.ToString();
    //这一行代码调用了 BindDataTable 方法，将 dtDate 变量中的数据绑定到 DataGridView 控件中。
    BindDataTable(dtDate);
}
//这段代码是一个事件处理函数，处理了一个按钮点击事件。btnCalculate的点击事件函数
private void btnCalculate_Click(object sender, EventArgs e)
{
    //检查
    //这段代码检查文本框 tbValue 中的值是否为空或者空字符串。
    //如果是空的，就会弹出一个消息框提示用户输入计算值，
    //并且函数提前结束（return），不再执行后面的代码。
    if (String.IsNullOrEmpty(this.tbValue.Text.ToString()))
    {
        MessageBox.Show("计算值不可为空，请输入");
        return;
    }
    //铁水废钢电炉冶炼
    if (cbType.Text.ToString() == "铁水废钢电炉冶炼")
    {
        //这段代码将文本框
        // tbValue 中的值解析为双精度浮点数，并将其存储在 value 变量中。
        double value;
        if (double.TryParse(this.tbValue.Text, out value) && value > 0)
        {
            //这行代码调用了 melting 对象的
            // GetMEC_HM_Calculate_Item 方法，获得一个 DataTable 对象 NewTable。
            DataTable NewTable = melting.GetMEC_HM_Calculate_Item();
            // 创建一个新的 DataColumn 对象，表示新列
            // 这个新列的名称是用户输入的计算值加上 "%铁水比CO2排放量(kg)"，
            // 类型是 double。
            DataColumn newColumn = new DataColumn(value.ToString() + "%铁水比CO2排放量(kg)", typeof(double));
            // 添加新列到 DataTable
            //然后将这个新列添加到 NewTable 中
            NewTable.Columns.Add(newColumn);
            // 遍历 NewTable 中的每一行，计算并添加新列的值
            foreach (DataRow row in NewTable.Rows)
            {
                //对于每一行，它从 "0%铁水比CO2排放量" 和
                // "30.14%铁水比CO2排放量" 两列中获取对应的值，并进行一定的计算。
                double HMCO2Emission_a;
                double HMCO2Emission_b;
                double.TryParse(row["0%铁水比CO2排放量"].ToString(), out HMCO2Emission_a);
                double.TryParse(row["30.14%铁水比CO2排放量"].ToString(), out HMCO2Emission_b);
                row[newColumn] = HMCO2Emission_a + (HMCO2Emission_b - HMCO2Emission_a) / 30.14 * (value);
            }
            BindCalculateDataTable(NewTable);
        }
    }
}

 public DataTable GetMEC_HM_Calculate(String Name)
 {
    // 创建一个 SqlOperator 对象，用于执行 SQL 语句，这个sql语句获取了一个表格中的一些数据
     StringBuilder strBuilder = new StringBuilder("SELECT 项目,[0%铁水比CO2排放量],[30.14%铁水比CO2排放量], [50.00%铁水比CO2排放量], [59.51%铁水比CO2排放量],[80.29%铁水比CO2排放量]  FROM " + Name);
     //可能是一个自定义类，用于执行 SQL 操作
     SqlOperator operation = new SqlOperator();
     //这一行代码调用 operation 对象的 ExecuteFill 方法执行 SQL 查询
     //最后，GetMEC_HM_Calculate 方法返回查询结果，应该是一个 DataTable 对象。
     return operation.ExecuteFill(strBuilder.ToString(), null);
 }
 //它从数据库中获取铁水废钢电炉冶炼相关的数据并返回一个 DataTable 对象
public DataTable GetMEC_HM_Calculate_Item()
{
    //这一行代码创建了一个 StringBuilder 对象 strBuilder，用于构建 SQL 查询语句。
    ////SQL 查询语句是从名为 MEC_HM 的表中选择项目、以及两列铁水比 CO2 排放量相关的数据。
    StringBuilder strBuilder = new StringBuilder("SELECT 项目,[0%铁水比CO2排放量],[30.14%铁水比CO2排放量] FROM MEC_HM");
    //这一行代码创建了一个 SqlOperator 对象 operation，
    //可能是用于执行 SQL 操作的类的一个实例化对象。
    SqlOperator operation = new SqlOperator();
    //这一行代码调用 operation 对象的 ExecuteFill 方法执行 SQL 查询，
    //并返回查询结果，应该是一个 DataTable 对象。
    //这个 DataTable 对象包含了铁水比 CO2 排放量相关的数据。
    return operation.ExecuteFill(strBuilder.ToString(), null);
}







