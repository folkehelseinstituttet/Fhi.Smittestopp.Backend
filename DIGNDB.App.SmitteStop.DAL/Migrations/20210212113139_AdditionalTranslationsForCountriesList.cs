using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class AdditionalTranslationsForCountriesList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "Code", "PullingFromGatewayEnabled", "VisitedCountriesEnabled" },
                values: new object[,]
                {
                    { 35L, "SA", false, false },
                    { 36L, "SO", false, false },
                    { 37L, "TI", false, false },
                    { 38L, "UR", false, false }
                });

            migrationBuilder.InsertData(
                table: "Translation",
                columns: new[] { "Id", "EntityId", "EntityName", "LanguageCountryId", "Value" },
                values: new object[,]
                {
                    { 137L, 17L, "Country", 21L, "Litwa" },
                    { 138L, 18L, "Country", 21L, "Luksemburg" },
                    { 139L, 19L, "Country", 21L, "Malta" },
                    { 140L, 20L, "Country", 21L, "Holandia" },
                    { 141L, 21L, "Country", 21L, "Polska" },
                    { 142L, 22L, "Country", 21L, "Portugalia" },
                    { 144L, 24L, "Country", 21L, "Słowacja" },
                    { 136L, 16L, "Country", 21L, "Łotwa" },
                    { 145L, 25L, "Country", 21L, "Słowenia" },
                    { 146L, 26L, "Country", 21L, "Hiszpania" },
                    { 147L, 27L, "Country", 21L, "Szwecja" },
                    { 148L, 30L, "Country", 21L, "Islandia" },
                    { 143L, 23L, "Country", 21L, "Rumunia" },
                    { 135L, 15L, "Country", 21L, "Włochy" },
                    { 133L, 13L, "Country", 21L, "Węgry" },
                    { 149L, 31L, "Country", 21L, "Lichtenstein" },
                    { 132L, 12L, "Country", 21L, "Grecja" },
                    { 131L, 11L, "Country", 21L, "Niemcy" },
                    { 130L, 10L, "Country", 21L, "Francja" },
                    { 129L, 9L, "Country", 21L, "Finlandia" },
                    { 128L, 8L, "Country", 21L, "Estonia" },
                    { 127L, 7L, "Country", 21L, "Dania`" },
                    { 126L, 6L, "Country", 21L, "Czechy" },
                    { 125L, 5L, "Country", 21L, "Cypr" },
                    { 124L, 4L, "Country", 21L, "Chorwacja" },
                    { 123L, 3L, "Country", 21L, "Bułgaria" },
                    { 122L, 2L, "Country", 21L, "Belgia" },
                    { 121L, 1L, "Country", 21L, "Austria" },
                    { 134L, 14L, "Country", 21L, "Irlandia" },
                    { 150L, 32L, "Country", 21L, "Szwajcaria" }
                });

            migrationBuilder.InsertData(
                table: "Translation",
                columns: new[] { "Id", "EntityId", "EntityName", "LanguageCountryId", "Value" },
                values: new object[,]
                {
                    { 91L, 1L, "Country", 35L, "النمسا" },
                    { 208L, 30L, "Country", 37L, "ኣይስላንድ" },
                    { 207L, 27L, "Country", 37L, "ሽዊደን" },
                    { 206L, 26L, "Country", 37L, "ስጳኛ" },
                    { 205L, 25L, "Country", 37L, "ስሎቬንያ" },
                    { 204L, 24L, "Country", 37L, "ስሎቫክያ" },
                    { 203L, 23L, "Country", 37L, "ሩማንያ" },
                    { 202L, 22L, "Country", 37L, "ፖርቱጓል" },
                    { 201L, 21L, "Country", 37L, "ፖላንድ" },
                    { 200L, 20L, "Country", 37L, "ሆላንድ" },
                    { 199L, 19L, "Country", 37L, "ማልታ" },
                    { 198L, 18L, "Country", 37L, "ላክሰንበርግ" },
                    { 197L, 17L, "Country", 37L, "ሊታወን" },
                    { 209L, 31L, "Country", 37L, "ሊሽተንስታይን" },
                    { 196L, 16L, "Country", 37L, "ላትቭያ" },
                    { 194L, 14L, "Country", 37L, "ኣይርላንድ" },
                    { 193L, 13L, "Country", 37L, "ሃንጋሪ" },
                    { 192L, 12L, "Country", 37L, "ግሪኽ" },
                    { 191L, 11L, "Country", 37L, "ጀርመን" },
                    { 190L, 10L, "Country", 37L, "ፈረንሳ" },
                    { 189L, 9L, "Country", 37L, "ፊንላንድ" },
                    { 188L, 8L, "Country", 37L, "ኤስትላንድ" },
                    { 187L, 7L, "Country", 37L, "ዴኒማርክ" },
                    { 186L, 6L, "Country", 37L, "ቸክ ረፓብሊክ" },
                    { 185L, 5L, "Country", 37L, "ቂፕሮስ" },
                    { 184L, 4L, "Country", 37L, "ክሮኣሽያ" },
                    { 183L, 3L, "Country", 37L, "ቡልጋርያ" },
                    { 195L, 15L, "Country", 37L, "ጥልያን" },
                    { 182L, 2L, "Country", 37L, "በልጁም" },
                    { 210L, 32L, "Country", 37L, "ስዊዘርላንድ" },
                    { 212L, 2L, "Country", 38L, "بیلجیئم" },
                    { 238L, 30L, "Country", 38L, "آئس لینڈ" },
                    { 237L, 27L, "Country", 38L, "سویڈن" },
                    { 236L, 26L, "Country", 38L, "سپین" },
                    { 235L, 25L, "Country", 38L, "سلووینیا" },
                    { 234L, 24L, "Country", 38L, "سلوواکیہ" },
                    { 233L, 23L, "Country", 38L, "رومانیہ" },
                    { 232L, 22L, "Country", 38L, "پرتگال" },
                    { 231L, 21L, "Country", 38L, "پولینڈ" },
                    { 230L, 20L, "Country", 38L, "ہالینڈ" },
                    { 229L, 19L, "Country", 38L, "مالٹا" },
                    { 228L, 18L, "Country", 38L, "لکسمبرگ" },
                    { 227L, 17L, "Country", 38L, "لتھوئینیا" },
                    { 211L, 1L, "Country", 38L, "آسٹریا" },
                    { 226L, 16L, "Country", 38L, "لیٹویا" },
                    { 224L, 14L, "Country", 38L, "آئرلینڈ" },
                    { 223L, 13L, "Country", 38L, "ہنگری" },
                    { 222L, 12L, "Country", 38L, "یونان" },
                    { 221L, 11L, "Country", 38L, "جرمنی" },
                    { 220L, 10L, "Country", 38L, "فرانس" },
                    { 219L, 9L, "Country", 38L, "فن لینڈ" },
                    { 218L, 8L, "Country", 38L, "ایسٹونیا" },
                    { 217L, 7L, "Country", 38L, "ڈنمارک" },
                    { 216L, 6L, "Country", 38L, "چیک ریپبلک" },
                    { 215L, 5L, "Country", 38L, "قبرص" },
                    { 214L, 4L, "Country", 38L, "کرویشیا" },
                    { 213L, 3L, "Country", 38L, "بلغاریہ" },
                    { 225L, 15L, "Country", 38L, "اٹلی" },
                    { 181L, 1L, "Country", 37L, "ኣውስትርያ" },
                    { 180L, 32L, "Country", 36L, "Iswiserland" },
                    { 179L, 31L, "Country", 36L, "Liechtenstein" },
                    { 117L, 27L, "Country", 35L, "السوید" },
                    { 116L, 26L, "Country", 35L, "اسبانیا" },
                    { 115L, 25L, "Country", 35L, "سلوفینیا" },
                    { 114L, 24L, "Country", 35L, "سلوفاکیا" },
                    { 113L, 23L, "Country", 35L, "رومانیا" },
                    { 112L, 22L, "Country", 35L, "برتغال" },
                    { 111L, 21L, "Country", 35L, "(بولندا) بولونیا " },
                    { 110L, 20L, "Country", 35L, "هولندا" },
                    { 109L, 19L, "Country", 35L, "مالطا" },
                    { 108L, 18L, "Country", 35L, "لکسمبورج" },
                    { 107L, 17L, "Country", 35L, "لیتوانیا" },
                    { 106L, 16L, "Country", 35L, "لاتفیا" },
                    { 118L, 30L, "Country", 35L, "ایسلندة" },
                    { 105L, 15L, "Country", 35L, "إیطالیا" },
                    { 103L, 13L, "Country", 35L, "هنغاریا (المجر)" },
                    { 102L, 12L, "Country", 35L, "الیونان" },
                    { 101L, 11L, "Country", 35L, "ألمانیا" },
                    { 100L, 10L, "Country", 35L, "فرنسا" },
                    { 99L, 9L, "Country", 35L, "فنلندة" },
                    { 98L, 8L, "Country", 35L, "استونیا" },
                    { 97L, 7L, "Country", 35L, "دانمارك" },
                    { 96L, 6L, "Country", 35L, "تشیکیا" },
                    { 95L, 5L, "Country", 35L, "قبرص" },
                    { 94L, 4L, "Country", 35L, "کرواتیا" },
                    { 93L, 3L, "Country", 35L, "بلغاریا" },
                    { 92L, 2L, "Country", 35L, "بلجیکا" },
                    { 104L, 14L, "Country", 35L, "ایرلندا" },
                    { 119L, 31L, "Country", 35L, "لشتنشتاین" },
                    { 120L, 32L, "Country", 35L, "سویسرا" },
                    { 151L, 1L, "Country", 36L, "Awstriya" },
                    { 178L, 30L, "Country", 36L, "Island" },
                    { 177L, 27L, "Country", 36L, "Iswiidhan" },
                    { 176L, 26L, "Country", 36L, "Isbaaniya" },
                    { 175L, 25L, "Country", 36L, "Islofeeniya" },
                    { 174L, 24L, "Country", 36L, "Islofaakiya" },
                    { 173L, 23L, "Country", 36L, "Romaaniya" },
                    { 172L, 22L, "Country", 36L, "Burtuqaal" },
                    { 171L, 21L, "Country", 36L, "Booland" },
                    { 170L, 20L, "Country", 36L, "Holland" },
                    { 169L, 19L, "Country", 36L, "Malta" },
                    { 168L, 18L, "Country", 36L, "Luksemburg" },
                    { 167L, 17L, "Country", 36L, "Litweyniya" },
                    { 166L, 16L, "Country", 36L, "Latfiya" },
                    { 165L, 15L, "Country", 36L, "Talyaaniga" },
                    { 164L, 14L, "Country", 36L, "Irland" },
                    { 163L, 13L, "Country", 36L, "Hangari" },
                    { 162L, 12L, "Country", 36L, "Giriiga" },
                    { 161L, 11L, "Country", 36L, "Jarmalka" },
                    { 160L, 10L, "Country", 36L, "Faransiiska" },
                    { 159L, 9L, "Country", 36L, "Finland" },
                    { 158L, 8L, "Country", 36L, "Estooniya" },
                    { 157L, 7L, "Country", 36L, "Denmark" },
                    { 156L, 6L, "Country", 36L, "Jeekiya" },
                    { 155L, 5L, "Country", 36L, "Sayberes" },
                    { 154L, 4L, "Country", 36L, "Kuruweeshiya" },
                    { 153L, 3L, "Country", 36L, "Bulgaariya" },
                    { 152L, 2L, "Country", 36L, "Beljim" },
                    { 239L, 31L, "Country", 38L, "لیختنستائین" },
                    { 240L, 32L, "Country", 38L, "سوئٹزرلینڈ" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 91L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 92L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 93L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 94L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 95L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 96L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 97L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 98L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 99L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 100L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 101L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 102L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 103L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 104L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 105L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 106L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 107L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 108L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 109L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 110L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 111L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 112L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 113L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 114L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 115L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 116L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 117L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 118L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 119L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 120L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 121L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 122L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 123L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 124L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 125L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 126L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 127L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 128L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 129L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 130L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 131L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 132L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 133L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 134L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 135L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 136L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 137L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 138L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 139L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 140L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 141L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 142L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 143L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 144L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 145L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 146L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 147L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 148L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 149L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 150L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 151L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 152L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 153L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 154L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 155L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 156L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 157L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 158L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 159L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 160L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 161L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 162L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 163L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 164L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 165L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 166L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 167L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 168L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 169L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 170L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 171L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 172L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 173L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 174L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 175L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 176L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 177L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 178L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 179L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 180L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 181L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 182L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 183L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 184L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 185L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 186L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 187L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 188L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 189L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 190L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 191L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 192L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 193L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 194L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 195L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 196L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 197L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 198L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 199L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 200L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 201L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 202L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 203L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 204L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 205L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 206L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 207L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 208L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 209L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 210L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 211L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 212L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 213L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 214L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 215L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 216L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 217L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 218L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 219L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 220L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 221L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 222L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 223L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 224L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 225L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 226L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 227L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 228L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 229L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 230L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 231L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 232L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 233L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 234L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 235L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 236L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 237L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 238L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 239L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 240L);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 35L);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 36L);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 37L);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 38L);
        }
    }
}
